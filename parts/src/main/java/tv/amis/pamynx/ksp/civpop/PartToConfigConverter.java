package tv.amis.pamynx.ksp.civpop;

import java.io.ByteArrayInputStream;
import java.io.IOException;
import java.io.InputStream;
import java.net.URISyntaxException;
import java.nio.charset.StandardCharsets;
import java.nio.file.Files;
import java.nio.file.Paths;
import java.util.ArrayList;
import java.util.List;
import java.util.stream.Collectors;

import tv.amis.pamynx.ksp.civpop.beans.KspModelField;
import tv.amis.pamynx.ksp.civpop.beans.KspPart;
import tv.amis.pamynx.ksp.civpop.beans.KspPartField;

public class PartToConfigConverter {

	
	public InputStream toConfig(KspPart part) throws IOException, URISyntaxException {
		String config = new String(Files.readAllBytes(Paths.get(getClass().getClassLoader().getResource("part-template.cfg").toURI())));

		for (KspPartField f : KspPartField.values()) {
			if (part.get(f) != null) {
				config = config.replaceAll("%"+f.name()+"%", part.get(f));
			}
		}
		config = config.replaceAll("%MODEL%", getBlockModel(part));
		config = config.replaceAll("%node_stack%", getBlockNodeStack(part));
		return new ByteArrayInputStream(config.getBytes(StandardCharsets.UTF_8));
	}

	private String getBlockModel(KspPart part) {
		List<String> res = new ArrayList<>();
		if (part.getModel() != null) {
			res.add("    MODEL");
			res.add("    {");
			res.add("        model = "+part.getModel().get(KspModelField.model));
			res.add("    }");
		}
		return res.stream().collect(Collectors.joining("\n"));
	}

	private String getBlockNodeStack(KspPart part) {
		List<String> res = new ArrayList<>();
		for (KspPartField f : KspPartField.values()) {
			if (f.name().startsWith("node_stack") && part.get(f) != null) {
				res.add("    "+f.name()+" = "+part.get(f));
			}
		}
		return res.stream().collect(Collectors.joining("\n"));
	}
}
