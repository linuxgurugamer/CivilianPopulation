package tv.amis.pamynx.ksp.civpop;

import java.io.IOException;
import java.net.URISyntaxException;
import java.nio.file.Files;
import java.nio.file.Path;
import java.nio.file.Paths;
import java.nio.file.StandardCopyOption;

import org.apache.poi.ss.usermodel.Workbook;
import org.apache.poi.xssf.usermodel.XSSFWorkbook;
import org.slf4j.Logger;
import org.slf4j.LoggerFactory;

import tv.amis.pamynx.ksp.civpop.beans.KspPart;
import tv.amis.pamynx.ksp.civpop.repository.KspRepository;

public class ConfigBuilder {

	private final Logger logger = LoggerFactory.getLogger(this.getClass());

	private Path target;
	private KspRepository repository;
	private PartToConfigConverter converter;

	public static final String LINE_SEPARATOR = System.getProperty("line.separator");

	public ConfigBuilder(String target) {
		this.target = Paths.get(target);
		this.repository = new KspRepository();
		this.converter = new PartToConfigConverter(repository);
	}
	
	public static void main(String...args) throws IOException {
		ConfigBuilder builder = new ConfigBuilder(args[0]);
		builder.build(args[1]);
	}

	public void build(String data) throws IOException {
		logger.info("building part file from "+data+" to "+target+"...");
		try (Workbook wb = new XSSFWorkbook(this.getClass().getClassLoader().getResourceAsStream("PARTS.xlsx"))){
			repository.load(wb);
		};
		repository.getParts().stream()
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
