#!/bin/sh
rm -rf /Users/rleroy/Games/KSP/1.3/KSP-Sandbox/ksp-civilian-population-mod-*
rm -rf /Users/rleroy/Games/KSP/1.3/KSP-Sandbox/GameData/CivilianPopulation
cp /Users/rleroy/git/CivilianPopulation/mod/target/ksp-civilian-population-mod-*.zip /Users/rleroy/Games/KSP/1.3/KSP-Sandbox/GameData/.
cd /Users/rleroy/Games/KSP/1.3/KSP-Sandbox/GameData
unzip ksp-civilian-population-mod-*.zip
