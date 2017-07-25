#!/bin/sh
rm -rf /Users/rleroy/Games/KSP/1.3/KSP-with-addons/ksp-civilian-population-mod-*
rm -rf /Users/rleroy/Games/KSP/1.3/KSP-with-addons/GameData/CivilianPopulation
cp /Users/rleroy/git/CivilianPopulation/mod/target/ksp-civilian-population-mod-*.zip /Users/rleroy/Games/KSP/1.3/KSP-with-addons/GameData/.
cd /Users/rleroy/Games/KSP/1.3/KSP-with-addons/GameData
unzip ksp-civilian-population-mod-*.zip
