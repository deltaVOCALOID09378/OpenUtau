@echo off
:: ============================================================================
:: Made And Checked By DELTA SYNTH & Gemini AI
:: Original by Patiphat Wongyai (Delta)
:: Version: v1.1 | Date: 2026-08-09
:: Function: Convert Thai DiffSinger Phoneme Dictionary to Thai VCCV Format.
:: ============================================================================

chcp 65001 >nul
setlocal enabledelayedexpansion

title DELTA SYNTH - DiffSinger to VCCV Dictionary Converter v1.1
color 0B

echo ============================================================================
echo [OptiLink System] Thai DiffSinger to Thai VCCV Phonemizer Converter v1.1
echo ============================================================================
echo.

:: Get Start Time
for /f "tokens=*" %%a in ('powershell -NoProfile -Command "Get-Date -Format 'HH:mm:ss'"') do set "START_TIME=%%a"
echo [SYSTEM] Process started at: %START_TIME%
echo.

:: Check for Drag and Drop inputs
set "TARGET_FILES="
if "%~1" neq "" (
    set "TARGET_FILES=%*"
) else (
    if exist "words_th_dict.txt" (
        set "TARGET_FILES="words_th_dict.txt""
    ) else (
        echo [ERROR] No input files provided and 'words_th_dict.txt' not found.
        echo [INFO] Please drag and drop dictionary files onto this script.
        echo.
        pause
        exit /b 1
    )
)

:: PowerShell Embedded Script for exact token mapping and UTF-8 safety
set "PS_SCRIPT=$ErrorActionPreference = 'Stop'; $failed = $false; "
set "PS_SCRIPT=!PS_SCRIPT! $map = New-Object 'System.Collections.Generic.Dictionary[String,String]' ([System.StringComparer]::Ordinal); "
:: DiffSinger to VCCV Mapping
set "PS_SCRIPT=!PS_SCRIPT! $map.Add('ng', 'g'); $map.Add('kk', 'k'); $map.Add('k', 'kh'); $map.Add('pp', 'p'); $map.Add('p', 'ph'); $map.Add('tt', 't'); $map.Add('t', 'th'); "
set "PS_SCRIPT=!PS_SCRIPT! $map.Add('A', '@'); $map.Add('O', 'Q'); $map.Add('E', '3'); $map.Add('Ua', '6'); $map.Add('U', '1'); $map.Add('au', 'aw'); $map.Add('I', 'ay'); "
set "PS_SCRIPT=!PS_SCRIPT! $map.Add('M', 'm'); $map.Add('Y', 'y'); $map.Add('W', 'w'); $map.Add('K', 'k'); $map.Add('B', 'b'); $map.Add('D', 'd'); "

set "PS_SCRIPT=!PS_SCRIPT! foreach ($file in @($env:OPENUTAU_CONVERTER_FILE)) { "
set "PS_SCRIPT=!PS_SCRIPT!   if (-Not (Test-Path -LiteralPath $file)) { Write-Host \"[ERROR] File not found: $file\" -ForegroundColor Red; $failed = $true; continue; } "
set "PS_SCRIPT=!PS_SCRIPT!   $outPath = [System.IO.Path]::GetDirectoryName($file) + '\' + [System.IO.Path]::GetFileNameWithoutExtension($file) + '_VCCV.txt'; "
set "PS_SCRIPT=!PS_SCRIPT!   Write-Host \"[PROCESSING] Reading: $file\" -ForegroundColor Cyan; "
set "PS_SCRIPT=!PS_SCRIPT!   $lines = Get-Content -Path $file -Encoding UTF8; "
set "PS_SCRIPT=!PS_SCRIPT!   $outData = @(); "
set "PS_SCRIPT=!PS_SCRIPT!   foreach ($line in $lines) { "
set "PS_SCRIPT=!PS_SCRIPT!     if ([string]::IsNullOrWhiteSpace($line) -or $line.Trim().StartsWith('#')) { continue; } "
:: Split by tab, space, or '=' to separate the Thai word from phonemes
set "PS_SCRIPT=!PS_SCRIPT!     $parts = $line -split '[\t =]+'; "
set "PS_SCRIPT=!PS_SCRIPT!     if ($parts.Length -lt 2) { continue; } "
set "PS_SCRIPT=!PS_SCRIPT!     $thaiWord = $parts[0]; "
set "PS_SCRIPT=!PS_SCRIPT!     $convertedPhonemes = @(); "
set "PS_SCRIPT=!PS_SCRIPT!     for ($i = 1; $i -lt $parts.Length; $i++) { "
set "PS_SCRIPT=!PS_SCRIPT!       $token = $parts[$i]; "
set "PS_SCRIPT=!PS_SCRIPT!       if ($map.ContainsKey($token)) { $convertedPhonemes += $map[$token]; } else { $convertedPhonemes += $token; } "
set "PS_SCRIPT=!PS_SCRIPT!     } "
set "PS_SCRIPT=!PS_SCRIPT!     $finalPhonemes = $convertedPhonemes -join ' '; "
:: Format for VCCV: word=ph1 ph2
set "PS_SCRIPT=!PS_SCRIPT!     $newLine = \"$thaiWord=$finalPhonemes\"; "
set "PS_SCRIPT=!PS_SCRIPT!     $outData += $newLine; "
set "PS_SCRIPT=!PS_SCRIPT!     Write-Host \"   [CONVERTED] $line  ->  $newLine\" -ForegroundColor DarkGray; "
set "PS_SCRIPT=!PS_SCRIPT!   } "
set "PS_SCRIPT=!PS_SCRIPT!   [System.IO.File]::WriteAllLines($outPath, $outData, [System.Text.Encoding]::UTF8); "
set "PS_SCRIPT=!PS_SCRIPT!   Write-Host \"[SUCCESS] Saved to: $outPath`n\" -ForegroundColor Green; "
set "PS_SCRIPT=!PS_SCRIPT! } if ($failed) { exit 1 } "

:: Execute the embedded PowerShell script
set "CONVERT_EXIT_CODE=0"
for %%F in (%TARGET_FILES%) do (
    set "OPENUTAU_CONVERTER_FILE=%%~fF"
    powershell -NoProfile -ExecutionPolicy Bypass -Command "!PS_SCRIPT!"
    if errorlevel 1 set "CONVERT_EXIT_CODE=!errorlevel!"
)
set "OPENUTAU_CONVERTER_FILE="
if not "!CONVERT_EXIT_CODE!"=="0" (
    echo [ERROR] One or more dictionary conversions failed.
    pause
    exit /b !CONVERT_EXIT_CODE!
)

:: Get End Time
for /f "tokens=*" %%a in ('powershell -NoProfile -Command "Get-Date -Format 'HH:mm:ss'"') do set "END_TIME=%%a"
echo ============================================================================
echo [SYSTEM] Process completed at: %END_TIME%
echo [SYSTEM] All conversions finished successfully with 0 errors.
echo ============================================================================
echo.
pause
exit /b 0