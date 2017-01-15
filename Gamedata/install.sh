#!/bin/sh
rm -rf /Volumes/DATA2/KSP/Games/1.2.2/KSP-Sandbox/GameData/CivilianPopulation
cd /Volumes/DATA/git/CivilianPopulation/Gamedata
tar -czvf CivilianPopulation.tgz CivilianPopulation
cp CivilianPopulation.tgz /Volumes/DATA2/KSP/Games/1.2.2/KSP-Sandbox/GameData/.
cd /Volumes/DATA2/KSP/Games/1.2.2/KSP-Sandbox/GameData
tar -xzvf CivilianPopulation.tgz
