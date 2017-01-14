package tv.amis.pamynx.ksp.civpop;

import java.io.IOException;
import java.net.URISyntaxException;
import java.nio.file.Files;
import java.nio.file.Path;
import java.nio.file.Paths;
import java.nio.file.StandardCopyOption;

import org.slf4j.Logger;
import org.slf4j.LoggerFactory;

import tv.amis.pamynx.ksp.civpop.beans.KspPart;

public class ConfigBuilder {

	private final Logger logger = LoggerFactory.getLogger(this.getClass());

	private Path target;
	
	private DataLoader loader;
	private PartToConfigConverter converter;

	public ConfigBuilder(String target) {
		this.target = Paths.get(target);
		this.loader = new DataLoader();
		this.converter = new PartToConfigConverter();
	}

	public void build(String data) {
		loader.load(data).stream()
			.forEach(this::writePartConfig);
	}

	private void writePartConfig(KspPart part) {
		try {
			Path partTarget = getPartTargetConfigPath(part);
			partTarget.getParent().toFile().mkdirs();
			Files.copy(converter.toConfig(part), partTarget, StandardCopyOption.REPLACE_EXISTING);
		} catch (IOException | URISyntaxException e) {
			logger.error(e.getMessage(),  e);
			throw new ConfigBuilderException(e);
		}
	}

	private Path getPartTargetConfigPath(KspPart part) {
		return this.target.resolve("Parts").resolve(part.getCategory()).resolve(part.getName()+".cfg");
	}

}
