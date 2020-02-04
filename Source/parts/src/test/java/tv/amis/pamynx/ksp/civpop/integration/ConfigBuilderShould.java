package tv.amis.pamynx.ksp.civpop.integration;

import static tv.amis.pamynx.ksp.civpop.ConfigBuilder.LINE_SEPARATOR;

import java.io.File;
import java.io.IOException;
import java.net.URISyntaxException;
import java.nio.charset.Charset;
import java.nio.file.Files;
import java.nio.file.Paths;
import java.util.List;
import java.util.stream.Collectors;

import org.hamcrest.Matchers;
import org.junit.Assert;
import org.junit.Test;

public class ConfigBuilderShould {

	private static final String path = "./target/Gamedata/CivilianPopulation";
	
	@Test
	public void create_parts_folder() throws IOException, URISyntaxException {
		File parts = new File(path+"/Parts/");
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
		checkPart("megadrill", "Utility");
		checkPart("pipeNetwork", "Utility");
		
	}
	
	private void checkPart(String partName, String category) throws IOException, URISyntaxException {
		File part = new File(path+"/Parts/"+category+"/"+partName+".cfg");
		Assert.assertTrue(partName + " was not created !", part.exists());
		List<String> actualLines = Files.readAllLines(part.toPath()).stream().map(s -> s.replaceAll("  ", " ")).collect(Collectors.toList());
		List<String> expectedLines = Files.readAllLines(Paths.get(getClass().getClassLoader().getResource(partName+"-expected.cfg").toURI()))
				.stream().map(s -> s.replaceAll("  ", " ")).collect(Collectors.toList());
		Assert.assertEquals(actualLines, expectedLines);
	}
}
