# แผนผังโปรเจกต์ (วันที่ 12.07.2569) 
```text 
Folder PATH listing for volume Other Data
Volume serial number is 00000198 6E3F:4AEC
A:\OPENUTAU-LUNAI-0.2.0.0\OPENUTAU.CORE
|   BaseChinesePhonemizer.cs
|   DefaultPhonemizer.cs
|   DocManager.cs
|   KoreanPhonemizerUtil.cs
|   MachineLearningPhonemizer.cs
|   OpenUtau.Core.csproj
|   PackageManager.cs
|   PlaybackManager.cs
|   SingerManager.cs
|   
+---All List File Name
|       A List of the All File-12.07.2569.md
|       
+---Analysis
|   |   AudioSlicer.cs
|   |   Game.cs
|   |   MidiExtractor.cs
|   |   Rmvpe.cs
|   |   Some.cs
|   |   TranscribedNote.cs
|   |   
|   \---Crepe
|           Crepe.cs
|           LICENSE.txt
|           Resources.Designer.cs
|           Resources.resx
|           tiny.onnx
|           
+---Api
|       G2pDictionary.cs
|       G2pDictionaryData.cs
|       G2pFallbacks.cs
|       G2pPack.cs
|       G2pRemapper.cs
|       IG2p.cs
|       Phonemizer.cs
|       PhonemizerFactory.cs
|       PhonemizerInstaller.cs
|       PhonemizerRunner.cs
|       README.md
|       
+---Audio
|       DummyAudioOutput.cs
|       IAudioOutput.cs
|       MiniAudioOutput.cs
|       
+---bin
|   +---Debug
|   |   \---net8.0
|   |           Melanchall_DryWetMidi_Native32.dll
|   |           Melanchall_DryWetMidi_Native64.dll
|   |           Melanchall_DryWetMidi_Native64.dylib
|   |           OpenUtau.Core.deps.json
|   |           OpenUtau.Core.dll
|   |           
|   \---Release
|       \---net8.0
|               Melanchall_DryWetMidi_Native32.dll
|               Melanchall_DryWetMidi_Native64.dll
|               Melanchall_DryWetMidi_Native64.dylib
|               OpenUtau.Core.deps.json
|               OpenUtau.Core.dll
|               
+---Classic
|   |   ClassicRenderer.cs
|   |   ClassicSinger.cs
|   |   ClassicSingerLoader.cs
|   |   ExeInstaller.cs
|   |   ExeResampler.cs
|   |   ExeWavtool.cs
|   |   Frq.cs
|   |   Ini.cs
|   |   IPlugin.cs
|   |   IResampler.cs
|   |   IWavtool.cs
|   |   OtoWatcher.cs
|   |   Plugin.cs
|   |   PluginLoader.cs
|   |   PluginRunner.cs
|   |   Presamp.cs
|   |   ResamplerItem.cs
|   |   ResamplerManifest.cs
|   |   SharpWavtool.cs
|   |   ToolsManager.cs
|   |   Ust.cs
|   |   UstNote.cs
|   |   VoiceBank.cs
|   |   VoicebankConfig.cs
|   |   VoicebankErrorChecker.cs
|   |   VoicebankFiles.cs
|   |   VoicebankInstaller.cs
|   |   VoicebankLoader.cs
|   |   VoicebankPublisher.cs
|   |   WorldlineRenderer.cs
|   |   WorldlineResampler.cs
|   |   
|   +---Data
|   |       mel.onnx
|   |       Resources.Designer.cs
|   |       Resources.resx
|   |       
|   \---Flags
|           UstFlag.cs
|           UstFlagParser.cs
|           
+---Commands
|       ExpCommands.cs
|       NoteCommands.cs
|       Notifications.cs
|       PartCommands.cs
|       ProjectCommands.cs
|       TrackCommands.cs
|       UCommand.cs
|       
+---DiffSinger
|   |   DiffSingerBasePhonemizer.cs
|   |   DiffSingerCache.cs
|   |   DiffSingerConfig.cs
|   |   DiffSingerPitch.cs
|   |   DiffSingerRealCurveScheduler.cs
|   |   DiffSingerRenderer.cs
|   |   DiffSingerRetake.cs
|   |   DiffSingerScript.cs
|   |   DiffSingerSinger.cs
|   |   DiffSingerSpeakerEmbedManager.cs
|   |   DiffSingerUnvoicedConfig.cs
|   |   DiffSingerUnvoicedConsonantPatch.cs
|   |   DiffSingerUtils.cs
|   |   DiffSingerVariance.cs
|   |   DiffSingerVariancePatch.cs
|   |   DiffSingerVocoder.cs
|   |   LunaiDsUnvoicedDefaults.cs
|   |   
|   +---Data
|   |       lunai-dsunvoiced.yaml
|   |       
|   \---Phonemizers
|           DiffSingerARPAPlusEnglishPhonemizer.cs
|           DiffSingerBrapaPhonemizer.cs
|           DiffSingerChinesePhonemizer.cs
|           DiffSingerEnglishPhonemizer.cs
|           DiffSingerFilipinoPhonemizer.cs
|           DiffSingerFrenchMillefeuillePhonemizer.cs
|           DiffSingerG2pPhonemizer.cs
|           DiffSingerGermanMarzipanPhonemizer.cs
|           DiffSingerGermanPhonemizer.cs
|           DiffSingerItalianPhonemizer.cs
|           DiffSingerJapanesePhonemizer.cs
|           DiffSingerJyutpingPhonemizer.cs
|           DiffSingerKoreanG2PPhonemizer.cs
|           DiffSingerKoreanPhonemizer.cs
|           DiffSingerPhonemizer.cs
|           DiffSingerPortuguesePhonemizer.cs
|           DiffSingerRefinedPhonemizer.cs
|           DiffSingerRhythmizerPhonemizer.cs
|           DiffSingerRuleBasedFilipinoPhonemizer.cs
|           DiffSingerRussianPhonemizer.cs
|           DiffSingerSpanishPhonemizer.cs
|           DiffSingerThaiPhonemizer.cs
|           DiffSingerUkrainianPhonemizer.cs
|           
+---Editing
|       BatchEdit.cs
|       HarmonyGenerator.cs
|       LyricBatchEdits.cs
|       LyricCleanerUtil.cs
|       NoteBatchEdits.cs
|       README.md
|       RealTimePitchGenerationService.cs
|       ResetBatchEdits.cs
|       
+---Enunu
|       EnunuClient.cs
|       EnunuConfig.cs
|       EnunuEnglishPhonemizer.cs
|       EnunuKoreanPhonemizer.cs
|       EnunuPhonemizer.cs
|       EnunuRenderer.cs
|       EnunuSinger.cs
|       EnunuUtils.cs
|       
+---Format
|   |   Commonnote.cs
|   |   Formats.cs
|   |   MidiWriter.cs
|   |   MusicXML.cs
|   |   OpusOggWaveReader.cs
|   |   ProjectImportOptions.cs
|   |   Svp.cs
|   |   SvpJsonModels.cs
|   |   SvpLyricsProcessor.cs
|   |   SvpNoteGapFiller.cs
|   |   SvpNoteOverlapResolver.cs
|   |   SvpPitchProcessor.cs
|   |   Ufdata.cs
|   |   USTx.cs
|   |   VSQx.cs
|   |   Wave.cs
|   |   
|   \---MusicXMLModel.cs
|           MusicXMLSchema.cs
|           ScorePartwisePartMeasure.cs
|           
+---G2p
|   |   ArpabetG2p.cs
|   |   ArpabetPlusG2p.cs
|   |   BrapaG2p.cs
|   |   FilipinoG2p.cs
|   |   FrenchG2p.cs
|   |   FrenchMillefeuilleG2p.cs
|   |   GermanG2p.cs
|   |   GermanMarzipanG2p.cs
|   |   ItalianG2p.cs
|   |   JapaneseMonophoneG2p.cs
|   |   KoreanG2p.cs
|   |   PortugueseG2p.cs
|   |   RussianG2p.cs
|   |   SpanishG2p.cs
|   |   ThaiG2p.cs
|   |   UkrainianG2p.cs
|   |   
|   \---Data
|           g2p-arpabet-plus.zip
|           g2p-arpabet.zip
|           g2p-brapa.zip
|           g2p-de-marzipan.zip
|           g2p-de.zip
|           g2p-es.zip
|           g2p-fil.zip
|           g2p-fr-millefeuille.zip
|           g2p-fr.zip
|           g2p-it.zip
|           g2p-ja-mono.zip
|           g2p-ko.zip
|           g2p-pt.zip
|           g2p-ru.zip
|           g2p-th.zip
|           g2p-uk.zip
|           Resources.Designer.cs
|           Resources.resx
|           
+---LunaiSingers
|       SingerHubClient.cs
|       
+---obj
|   |   OpenUtau.Core.csproj.nuget.dgspec.json
|   |   OpenUtau.Core.csproj.nuget.g.props
|   |   OpenUtau.Core.csproj.nuget.g.targets
|   |   project.assets.json
|   |   project.nuget.cache
|   |   
|   +---Debug
|   |   \---net8.0
|   |       |   .NETCoreApp,Version=v8.0.AssemblyAttributes.cs
|   |       |   OpenUtau.Core.Analysis.Crepe.Resources.resources
|   |       |   OpenUtau.Core.AssemblyInfo.cs
|   |       |   OpenUtau.Core.AssemblyInfoInputs.cache
|   |       |   OpenUtau.Core.assets.cache
|   |       |   OpenUtau.Core.Classic.Data.Resources.resources
|   |       |   OpenUtau.Core.csproj.AssemblyReference.cache
|   |       |   OpenUtau.Core.csproj.CoreCompileInputs.cache
|   |       |   OpenUtau.Core.csproj.FileListAbsolute.txt
|   |       |   OpenUtau.Core.csproj.GenerateResource.cache
|   |       |   OpenUtau.Core.dll
|   |       |   OpenUtau.Core.G2p.Data.Resources.resources
|   |       |   OpenUtau.Core.GeneratedMSBuildEditorConfig.editorconfig
|   |       |   OpenUtau.Core.Vogen.Data.VogenRes.resources
|   |       |   
|   |       +---ref
|   |       |       OpenUtau.Core.dll
|   |       |       
|   |       \---refint
|   |               OpenUtau.Core.dll
|   |               
|   \---Release
|       \---net8.0
|           |   .NETCoreApp,Version=v8.0.AssemblyAttributes.cs
|           |   OpenUtau.Core.Analysis.Crepe.Resources.resources
|           |   OpenUtau.Core.AssemblyInfo.cs
|           |   OpenUtau.Core.AssemblyInfoInputs.cache
|           |   OpenUtau.Core.assets.cache
|           |   OpenUtau.Core.Classic.Data.Resources.resources
|           |   OpenUtau.Core.csproj.AssemblyReference.cache
|           |   OpenUtau.Core.csproj.CoreCompileInputs.cache
|           |   OpenUtau.Core.csproj.FileListAbsolute.txt
|           |   OpenUtau.Core.csproj.GenerateResource.cache
|           |   OpenUtau.Core.dll
|           |   OpenUtau.Core.G2p.Data.Resources.resources
|           |   OpenUtau.Core.GeneratedMSBuildEditorConfig.editorconfig
|           |   OpenUtau.Core.Vogen.Data.VogenRes.resources
|           |   PublishOutputs.d7bafa623d.txt
|           |   
|           +---ref
|           |       OpenUtau.Core.dll
|           |       
|           \---refint
|                   OpenUtau.Core.dll
|                   
+---Properties
|       AssemblyInfo.cs
|       
+---Render
|       IRenderer.cs
|       PhraseWaveformCache.cs
|       RealCurveUpdater.cs
|       RenderCache.cs
|       RenderEngine.cs
|       Renderers.cs
|       RenderPhrase.cs
|       Worldline.cs
|       
+---SignalChain
|   |   ExportAdapter.cs
|   |   Fader.cs
|   |   ISignalSource.cs
|   |   MasterAdapter.cs
|   |   MetronomeEngine.cs
|   |   MetronomeScheduler.cs
|   |   MixFxSource.cs
|   |   WaveMix.cs
|   |   WaveSource.cs
|   |   
|   \---Effects
|           BiquadEQ.cs
|           Freeverb.cs
|           FxPresets.cs
|           IEffect.cs
|           SimpleCompressor.cs
|           
+---ThirdParty
|       Deque.cs
|       F0Smoother.cs
|       
+---Ustx
|       UCurve.cs
|       UExpression.cs
|       UMixFx.cs
|       UNote.cs
|       UPart.cs
|       UPhoneme.cs
|       UProject.cs
|       USinger.cs
|       UTrack.cs
|       
+---Util
|       Base64.cs
|       IniFileClass.cs
|       KeySignatureHelper.cs
|       LibraryLoader.cs
|       LivePitchMode.cs
|       LocalizedSort.cs
|       LyricsHelper.cs
|       MessageCustomizableException.cs
|       MusicMath.cs
|       NotePresets.cs
|       Onnx.cs
|       OS.cs
|       PathManager.cs
|       PianoRollEditTool.cs
|       Preferences.cs
|       ProcessRunner.cs
|       SingletonBase.cs
|       SplineInterpolate.cs
|       SplitLyrics.cs
|       TimeAxis.cs
|       WindowSize.cs
|       Yaml.cs
|       Zip.cs
|       
+---VocalShaper
|       Complex.cs
|       VSMath.cs
|       VSVocoder.cs
|       World.cs
|       
+---Vogen
|   |   TrieNode.cs
|   |   VogenBasePhonemizer.cs
|   |   VogenMandarinPhonemizer.cs
|   |   VogenRenderer.cs
|   |   VogenSinger.cs
|   |   VogenSingerInstaller.cs
|   |   VogenSingerLoader.cs
|   |   VogenYuePhonemizer.cs
|   |   
|   \---Data
|           f0.man.onnx
|           f0.yue.onnx
|           g2p.man.onnx
|           g2p.yue.onnx
|           po.man.onnx
|           po.yue.onnx
|           VogenRes.Designer.cs
|           VogenRes.resx
|           yue.csv
|           
\---Voicevox
    |   VoicevoxClient.cs
    |   VoicevoxConfig.cs
    |   VoicevoxRenderer.cs
    |   VoicevoxSinger.cs
    |   VoicevoxUtils.cs
    |   
    \---Phonemizers
            SimpleVoicevoxPhonemizer.cs
            VoicevoxPhonemizer.cs
            
``` 
