package tv.amis.pamynx.ksp.civpop;

import java.io.File;
import java.io.IOException;
import java.net.URISyntaxException;
import java.nio.file.Files;
import java.nio.file.Paths;
import java.util.stream.Collectors;

import org.hamcrest.Matchers;
import org.junit.Assert;
import org.junit.Test;

public class ConfigBuilderShould {

	@Test
	public void create_parts_folder() throws IOException, URISyntaxException {
		ConfigBuilder builder = new ConfigBuilder("./target/Gamedata/CivilianPopulation");
		
		builder.build("PARTS.xlsx");
		
		File parts = new File("./target/Gamedata/CivilianPopulation/Parts/");
		Assert.assertThat(parts.exists(), Matchers.is(true));

		checkPart("bioDomeBase", "Structural");
		checkPart("bioDomeBaseLarge", "Structural");
		checkPart("bioSphereBase", "Structural");
		checkPart("bioSphereBaseNoWalls", "Structural");
		checkPart("bioSphereBaseWallRing", "Structural");
		checkPart("parkbioDomeBase", "Structural");
		checkPart("parkbioDomeBaseMetal", "Structural");
		checkPart("parkbioDomeBaseRock", "Structural");
		checkPart("stbiodomeFarmMk2", "Utility");
		checkPart("bioSphereWindows", "Structural");
		checkPart("bioSphereWindowsLarge", "Structural");
		checkPart("bioSphereWindowsWide", "Structural");
		checkPart("t1CivBiomassTank", "Utility");
		checkPart("CivPopReactor", "Electrical");
		checkPart("SmallCivPopReactor", "Electrical");
		checkPart("civieDock", "Utility");
		checkPart("t1civWasteWaterTank", "Utility");
		checkPart("t1CivWaterTank", "Utility");
		checkPart("colossalLandingLeg", "Ground");
		checkPart("colossalLandingLeg2", "Ground");
		checkPart("truss18x18NoCore", "Structural");
		checkPart("truss6x18", "Structural");
		checkPart("truss6x6Core", "Structural");
		checkPart("truss6x6CoreL", "Structural");
		checkPart("truss6x6CoreT", "Structural");
		checkPart("truss6x6CoreX", "Structural");
		checkPart("truss6x6NoCore", "Structural");
		checkPart("InsituKerbalRecruiterTest", "Utility");
		checkPart("t1CivilizationGenerationShipQuartersMedium", "Utility");
		checkPart("t1CivilizationGenerationShipQuartersLarge", "Utility");
		checkPart("surfaceAttachHouseSmall", "Utility");
		checkPart("nduniversity", "Science");
		checkPart("t1civSmallGardenModule", "Utility");
		checkPart("t1civGardenBiosphereMedium", "Utility");
		checkPart("t1civGardenBiosphere", "Utility");
		checkPart("megadrill", "Utility");
		checkPart("pipeNetwork", "Utility");
		
	}
	
	private void checkPart(String partName, String category) throws IOException, URISyntaxException {
		File part = new File("./target/Gamedata/CivilianPopulation/Parts/"+category+"/"+partName+".cfg");
		Assert.assertTrue(partName + " was not created !", part.exists());
		String actual = Files.readAllLines(part.toPath()).stream().collect(Collectors.joining("\n"));
		String expected = new String(Files.readAllBytes(Paths.get(getClass().getClassLoader().getResource(partName+"-expected.cfg").toURI())));
		Assert.assertEquals(expected, actual);
	}
}
