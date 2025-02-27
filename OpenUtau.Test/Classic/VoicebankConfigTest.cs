﻿using OpenUtau.Core;
using Xunit;
using Xunit.Abstractions;

namespace OpenUtau.Classic {
    public class VoicebankConfigTest {
        readonly ITestOutputHelper output;

        public VoicebankConfigTest(ITestOutputHelper output) {
            this.output = output;
        }

        static VoicebankConfig CreateConfig() {
            return new VoicebankConfig() {
                SymbolSet = new SymbolSet() {
                    Preset = SymbolSetPreset.hiragana,
                },
                Subbanks = new Subbank[] {
                    new Subbank() {
                        ToneRanges = new [] { "C1-C4" },
                    },
                    new Subbank() {
                        Suffix = "D4",
                        ToneRanges = new [] { "C#4-F4" },
                    },
                    new Subbank() {
                        Suffix = "G4",
                        ToneRanges = new [] { "F#4-A#4" },
                    },
                    new Subbank() {
                        Suffix = "C5",
                        ToneRanges = new [] { "B4-B7" },
                    },
                    new Subbank() {
                        Suffix = "C5P",
                        Color = "power" ,
                        ToneRanges = new [] { "B4-B7" },
                    },
                    new Subbank() {
                        Suffix = "C5S",
                        Color = "shout" ,
                        ToneRanges = new [] { "B4-B7" },
                    },
                }
            };
        }

        [Fact]
        public void SerializationTest() {
            var yaml = Yaml.DefaultSerializer.Serialize(CreateConfig());
            output.WriteLine(yaml);

            Assert.Equal(@"portrait_opacity: 0.67000001668930054
symbol_set:
  preset: hiragana
  head: '-'
  tail: R
subbanks:
- prefix: ''
  suffix: ''
  tone_ranges:
  - C1-C4
- prefix: ''
  suffix: D4
  tone_ranges:
  - C#4-F4
- prefix: ''
  suffix: G4
  tone_ranges:
  - F#4-A#4
- prefix: ''
  suffix: C5
  tone_ranges:
  - B4-B7
- color: power
  prefix: ''
  suffix: C5P
  tone_ranges:
  - B4-B7
- color: shout
  prefix: ''
  suffix: C5S
  tone_ranges:
  - B4-B7
".Replace("\r\n", "\n"), yaml.Replace("\r\n", "\n"));
        }

        [Fact]
        public void RoundTripTest() {
            var yaml = Yaml.DefaultSerializer.Serialize(CreateConfig());
            var config = Yaml.DefaultDeserializer.Deserialize<VoicebankConfig>(yaml);
            var yaml2 = Yaml.DefaultSerializer.Serialize(config);

            Assert.Equal(yaml, yaml2);
        }
    }
}
