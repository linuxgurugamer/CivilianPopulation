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

		File bioDomeBase = new File("./target/Gamedata/CivilianPopulation/Parts/Structural/bioDomeBase.cfg");
		Assert.assertThat(bioDomeBase.exists(), Matchers.is(true));
		String actual = Files.readAllLines(bioDomeBase.toPath()).stream().collect(Collectors.joining("\n"));
		String expected = new String(Files.readAllBytes(Paths.get(getClass().getClassLoader().getResource("bioDomeBase-expected.cfg").toURI())));
		Assert.assertEquals(expected, actual);
	}
}
