#!/bin/sh
rm -rf /Users/rleroy/Games/KSP/1.4.3/KSP-TACLS/ksp-civilian-population-mod-*
rm -rf /Users/rleroy/Games/KSP/1.4.3/KSP-TACLS/GameData/CivilianPopulation
cp /Users/rleroy/git/CivilianPopulation/mod/target/ksp-civilian-population-mod-*.zip /Users/rleroy/Games/KSP/1.4.3/KSP-TACLS/GameData/.
cd /Users/rleroy/Games/KSP/1.4.3/KSP-TACLS/GameData
unzip ksp-civilian-population-mod-*.zip
