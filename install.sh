#!/bin/sh
rm -rf /Users/rleroy/Games/KSP/1.3.1/KSP-sandbox/ksp-civilian-population-mod-*
rm -rf /Users/rleroy/Games/KSP/1.3.1/KSP-sandbox/GameData/CivilianPopulation
cp /Users/rleroy/git/CivilianPopulation/mod/target/ksp-civilian-population-mod-*.zip /Users/rleroy/Games/KSP/1.3.1/KSP-sandbox/GameData/.
cd /Users/rleroy/Games/KSP/1.3.1/KSP-sandbox/GameData
unzip ksp-civilian-population-mod-*.zip
