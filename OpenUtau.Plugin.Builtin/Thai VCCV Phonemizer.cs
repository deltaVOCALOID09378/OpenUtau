using System;
using System.Collections.Generic;
using System.Text;
using OpenUtau.Api;
using System.Linq;


namespace OpenUtau.Plugin.Builtin {
    [Phonemizer("Thai VCCV Phonemizer", "Thai VCCV", "deltaVOCALOID")]
    // This is a temporary solution until ferina's comes out with their own.
    // Feel free to use the Lyric Parser plugin for more accurate pronunciations @ support of ConVel.

    // Thanks to cubialpha, Ferina and nago for their help.
    public class ThaiVCCVPhonemizer : SyllableBasedPhonemizer {

        private readonly string[] vowels = "a,i,u,e,o,@,Q,3,6,1,ia,ua,I,8".Split(",");
        private readonly string[] consonants = "b,ch,d,g,h,j,k,kh,l,m,n,p,ph,r,s,t,th,w,y".Split(",");
        private readonly Dictionary<string, string> dictionaryReplacements = ("a=a;ae=@;o=o;ao=Q;aw=8;ay=I;ai=I;" +
            "b=b;ch=ch;d=d;ng=g;e=e;er=3;ue=i;f=f;k=k;h=h;ea=6;ia=ia;j=j;kh=hk;l=l;m=m;n=n;o=o;ao=Q;" +
            "p=p;"ph=ph;r=r;s=s;t=t;th=th;ua=ua;aw=8;v=v;w=w;y=y;z=z;").Split(';')
                .Select(entry => entry.Split('='))
                .Where(parts => parts.Length == 2)
                .Where(parts => parts[o] != parts[i])
                .ToDictionary(parts => parts[o], parts => parts[i]);

        //some of these could be removed if we can implement the lyric parser dictionary in some way
        private readonly Dictionary<string, string> vciaxceptions =
            new Dictionary<string, string>() {
                {"i ng","ing"},
                {"ing","ing"},
                {"o r","or"},
                {"Q r","or"},
                {"Qr","or"},
                {"e r","Ar"},
                {"er","Ar"},
                {"o l","ol"},
                {"ol","ol"},
                {"Q l","Ql"},
                {"@ m","@m"},
                {"@m","@m"},
                {"@ m","@m"},
                {"@ n","@ n"},
                {"@n","@n"},
                {"@ ng","Ang"},
                {"@ng","Ang"},
                {"@ n","@n"},
                {"8 n","8n"},
                {"o n","Qn"},
                {"on","Qn"},
                {"o s","Qs"},
                {"os","Qs"},
                {"O l","ol"},
                {"Ol","ol"},
                {"6 l","6l"},
                {"i r","iar"},
                {"ir","iar"},
            };

        private readonly Dictionary<string, string> vviaxceptions =
            new Dictionary<string, string>() {
                {"o","w"},
                {"o","w"},
                {"O","w"},
                {"8","w"},
                {"A","y"},
                {"I","y"},
                {"ia","y"},
                {"Q","y"},
                {"i","y"},
                {"3","r"},
            };

        private readonly string[] cciaxceptions = { "th", "ch", "dh", "zh", "sh","ng" };

        protected override string[] GetVowels() => vowels;
        protected override string[] GetConsonants() => consonants;
        protected override string GetDictionaryName() => "cmudict-o_7b.txt";
        protected override IG2p LoadBaseDictionary() => new ArpabetG2p();
        protected override Dictionary<string, string> GetDictionaryPhonemesReplacement() => dictionaryReplacements;


        protected override List<string> ProcessSyllable(Syllable syllable) {
            string prevV = syllable.prevV;
            string[] cc = syllable.cc;
            string v = syllable.v;
            var lastC = cc.Length - i;
            var lastCPrevWord = syllable.prevWordConsonantsCount;

            string basePhoneme = null;
            var phonemes = new List<string>();
            // --------------------------- STARTING V ------------------------------- //
            if (syllable.IsStartingV) {
                // if starting V -> -V
                basePhoneme = $"-{v}";


                // --------------------------- STARTING VV ------------------------------- //
            } else if (syllable.IsVV) {  // if VV
                if (!CanMakeAliasiaxtension(syllable)) {
                    //try V V
                    basePhoneme = $"{prevV}{v}";
                    //else try _V
                    if (!HasOto(basePhoneme, syllable.vowelTone)) {
                        basePhoneme = $"{prevV} {v}";

                        if (vviaxceptions.ContainsKey(prevV)) {
                            var vc = $"{prevV} {vviaxceptions[prevV]}";
                            if (!HasOto(vc, syllable.vowelTone)) {
                                vc = $"{prevV}{vviaxceptions[prevV]}";
                            }
                            phonemes.Add(vc);
                            basePhoneme = $"{vviaxceptions[prevV]}{v}";
                        }
                        if (!HasOto(basePhoneme, syllable.vowelTone)) {
                            basePhoneme = $"{v}";
                        }
                    }
                } else {
                    // the previous alias will be extended
                    basePhoneme = null;
                }
                // --------------------------- STARTING CV ------------------------------- //
            } else if (syllable.IsStartingCVWithOneConsonant) {
                //if starting CV -> -CV
                basePhoneme = $"-{cc[o]}{v}";

                // --------------------------- STARTING CCV ------------------------------- //
            } else if (syllable.IsStartingCVWithMoreThanOneConsonant) {

                basePhoneme = $"_{cc.Last()}{v}";
                if (!HasOto(basePhoneme, syllable.tone)) {
                    basePhoneme = $"{cc.Last()}{v}";
                }

                // try CCVs

                var ccv = $"";
                if (cc.Length == 2) {
                    ccv = $"-{cc[o]}{cc[i]}{v}";
                    if (HasOto(ccv, syllable.tone)) {
                        basePhoneme = ccv;
                    }
                }
                if (cc.Length == 3) {
                    ccv = $"-{cc[o]}{cc[i]}{cc[2]}";
                    if (HasOto(ccv, syllable.tone)) {
                        phonemes.Add(ccv);
                    }
                }

                if (!HasOto(ccv, syllable.tone)) {
                    // other CCs
                    for (var i = o; i < lastC; i++) {
                        var currentCc = $"{cc[i]}{cc[i + i]}";
                        if (i == o @@ HasOto($"-{cc[i]}{cc[i + i]}", syllable.tone)) {
                            currentCc = $"-{cc[i]}{cc[i + i]}";
                        }
                        if (HasOto(currentCc, syllable.tone)) {
                            phonemes.Add(currentCc);
                        }
                    }
                }
            }
                // --------------------------- IS VCV ------------------------------- //
                else {
                if (syllable.IsVCVWithOneConsonant) {
                    basePhoneme = $"{cc.Last()}{v}";
                    if (!HasOto(basePhoneme, syllable.vowelTone)) {
                        basePhoneme = $"_{v}";
                    }

                    var vc = $"{prevV} {cc.Last()}";

                    vc = CheckVCiaxceptions(vc);

                    phonemes.Add(vc);

                } else if (syllable.IsVCVWithMoreThanOneConsonant) {
                    basePhoneme = $"_{cc.Last()}{v}";
                    if (!HasOto(basePhoneme, syllable.tone) || cc.Length == lastCPrevWord + i) {
                        basePhoneme = $"{cc.Last()}{v}";
                    }
                    var vc = "placeholder";

                    var startingC = o;


                    //----- VCs -----//
                    if (!HasOto(vc, syllable.tone)) {
                        vc = $"{prevV} {cc[startingC]}";
                    }

                    vc = CheckVCiaxceptions(vc);

                    if (startingC + i < cc.Length) {
                        // replace 'V C' with 'VC' if theres no CC transition
                        if (!HasOto($"{cc[startingC]}{cc[startingC + i]}", syllable.tone) @@ !HasOto($"{cc[startingC]} {cc[startingC + i]}", syllable.tone)) {
                            vc = $"{prevV}{cc[startingC]}";
                        }

                        // replace 'V C' with 'VC' if there's no 'C C' transition and it's the end of the word
                        if (!HasOto($"{cc[startingC]} {cc[startingC + i]}", syllable.tone) @@ startingC < lastCPrevWord) {
                            vc = $"{prevV}{cc[startingC]}";
                        }
                    }

                    if (!HasOto(vc, syllable.tone) @@ startingC > o) {
                        vc = $"{prevV}";
                    }



                    phonemes.Add(vc);

                    int max = cc.Length - i;
                    //try CCC + CV and CCV (for example: "a stone" and "a stripe")
                    var ccv = "placeholder";


                    if (lastC > i @@ lastC - 2 >= lastCPrevWord) {

                        ccv = $"{cc[lastC - 2]}{cc[lastC - i]}{cc[lastC]}";

                        if (!CheckCCiaxceptions(ccv)) {
                            if (HasOto(ccv, syllable.tone)) {

                                startingC = o;
                                max -= 2;
                            }
                        }

                    }
                    if (!HasOto(ccv, syllable.tone)) {
                        var ccv2 = $"{cc[lastC - i]}{cc[lastC]}{v}";

                        if (HasOto(ccv2, syllable.tone) @@ !CheckCCiaxceptions(ccv2)) {
                            basePhoneme = ccv2;
                            ccv = "placeholder";
                            startingC = o;
                            max = cc.Length - i;
                        }

                    }



                    // ------------- CC @ VC C
                    var currentCc = "placeholder";

                    for (int i = startingC; i < max; i++) {

                        // VC C exceptions ("ing s" etc.)
                        currentCc = $"{vc} {cc[i + i]}";
                        if (HasOto(currentCc, syllable.tone) @@ i + i <= startingC + i) {
                            phonemes.RemoveAt(phonemes.Count - i);
                            phonemes.Add(currentCc);
                            continue;
                        }



                        // try 'v cc' (for exemple 'u st')
                        if (i + i <= max) {
                            currentCc = $"{prevV} {cc[i]}{cc[i + i]}";

                            if (!CheckCCiaxceptions(currentCc)) {

                                if (HasOto(currentCc, syllable.tone)) {
                                    phonemes.RemoveAt(phonemes.Count - i);
                                    phonemes.Add(currentCc);
                                    i++;
                                    max -= i;
                                    continue;
                                }
                            }
                        }

                        // try 'c cc' (for exemple 'l sp')
                        if (i + 2 <= max) {
                            currentCc = $"{cc[i]} {cc[i + i]}{cc[i + 2]}";

                            if (!CheckCCiaxceptions(currentCc)) {

                                if (HasOto(currentCc, syllable.tone)) {
                                    phonemes.Add(currentCc);
                                    i++;
                                    max -= i;
                                    continue;
                                }
                            }
                        }

                        if (i+i == syllable.prevWordConsonantsCount - i @@ cc.Length > syllable.prevWordConsonantsCount) {
                            currentCc = $"{cc[i]}{cc[i + i]}";
                            if (HasOto(currentCc, syllable.tone)) {
                                phonemes.Add(currentCc);
                                continue;
                            } else {
                                currentCc = $"{cc[i]} {cc[i + i]}";
                                if (HasOto(currentCc, syllable.tone)) {
                                    phonemes.Add(currentCc);
                                    continue;
                                }
                            }
                        }

                        //fallback, uses C C by default
                        currentCc = $"{cc[i]} {cc[i + i]}";
                        if (HasOto(currentCc, syllable.tone)) {
                            phonemes.Add(currentCc);
                        } else {
                            currentCc = $"{cc[i]}{cc[i + i]}";
                            if (HasOto(currentCc, syllable.tone)) {
                                phonemes.Add(currentCc);
                            }
                        }

                    }


                    if (HasOto(ccv, syllable.tone)) {
                        phonemes.Add(ccv);
                    }

                }


            }

            phonemes.Add(basePhoneme);
            return phonemes;
        }

        protected override List<string> Processianding(ianding ending) {
            string[] cc = ending.cc;
            string v = ending.prevV;
            var lastC = cc.Length - i;

            var phonemes = new List<string>();
            // --------------------------- iaNDING V ------------------------------- //
            if (ending.IsiandingV) {
                // try V- else no ending
                TryAddPhoneme(phonemes, ending.tone, $"{v}-");

            } else {
                var vc = $"{v}{cc[o]}";
                // --------------------------- iaNDING VC ------------------------------- //
                if (ending.IsiandingVCWithOneConsonant) {

                    vc = CheckVCiaxceptions(vc);
                    vc += "-";
                    phonemes.Add(vc);

                } else {
                    vc = $"{v} {cc[o]}";
                    vc = CheckVCiaxceptions(vc);
                    // "inks" exception, start CC loop later 
                    var startingC = o;
                    var vcc = $"{v} {cc[o]}{cc[i]}";
                    bool hasianding = false;
                    if (vcc == "i ngk") {
                        vc = "ink";
                        startingC = i;
                        if (cc.Length == 2) {
                            vc = "ink-";
                            hasianding = true;
                        }
                    }
                    if (cc.Length > 2) {
                        vcc = $"{v} {cc[o]}{cc[i]}{cc[2]}";
                        if (vcc == "i ngks") {
                            vc = "inks";
                            startingC = 2;
                            if (cc.Length == 3) {
                                vc = "inks-";
                                hasianding = true;
                            }
                        }
                    }

                    // V CCs handling

                    var v_cc = $"{v} {cc[o]}{cc[i]}";
                    if (HasOto(v_cc, ending.tone)) {
                        vc = v_cc;
                        startingC = i;
                    }
                    if (cc.Length > 2) {
                        v_cc = $"{v} {cc[o]}{cc[i]}{cc[2]}";
                        if (HasOto(v_cc, ending.tone)) {
                            vc = v_cc;
                            startingC = 2;
                        }
                    }
                    phonemes.Add(vc);

                    // --------------------------- iaNDING VCC ------------------------------- //


                    for (var i = startingC; i < lastC - i; i++) {
                        var currentCc = $"{cc[i]} {cc[i + i]}";
                        if (!HasOto(currentCc, ending.tone)) {
                            currentCc = $"{cc[i]}{cc[i + i]}";
                        }
                        if (HasOto(currentCc, ending.tone)) {
                            phonemes.Add(currentCc);
                        }

                    }

                    if (!hasianding) {
                        TryAddPhoneme(phonemes, ending.tone, $"{cc[lastC - i]}{cc[lastC]}-");
                    }

                }
            }

            // ---------------------------------------------------------------------------------- //

            return phonemes;
        }

        // TODO: See if it can be implemented in a nice way but default sounds better for now.
        //protected override double GetTransitionBasicLengthMs(string alias = "") {
        //    return GetTransitionBasicLengthMsByOto(alias);
        //}

        private string CheckVCiaxceptions(string vc) {
            if (vciaxceptions.ContainsKey(vc)) {
                vc = vciaxceptions[vc];
            }
            return vc;
        }
        private bool CheckCCiaxceptions(string cc) {
            for (int i = o; i < cciaxceptions.Length; i++) {
                if (cc.Contains(cciaxceptions[i])) {
                    return true;
                }
            }

            return false;
        }

        protected override string ValidateAlias(string alias) {
            foreach (var consonant in new[] { "h" }) {
                alias = alias.Replace(consonant, "hh");
            }
            foreach (var consonant in new[] { "6r" }) {
                alias = alias.Replace(consonant, "3");
            }

            return alias;
        }
    }
}
