package tv.amis.pamynx.ksp.civpop;

import java.io.ByteArrayInputStream;
import java.io.IOException;
import java.io.InputStream;
import java.net.URISyntaxException;
import java.nio.charset.StandardCharsets;
import java.nio.file.Files;
import java.nio.file.Paths;
import java.util.ArrayList;
import java.util.Arrays;
import java.util.List;
import java.util.stream.Collectors;

import tv.amis.pamynx.ksp.civpop.beans.KspModelField;
import tv.amis.pamynx.ksp.civpop.beans.KspPart;
import tv.amis.pamynx.ksp.civpop.beans.KspPartField;
import tv.amis.pamynx.ksp.civpop.beans.KspResource;
import tv.amis.pamynx.ksp.civpop.beans.KspResourceField;
import tv.amis.pamynx.ksp.civpop.beans.KspResourceType;

public class PartToConfigConverter {

	
	public InputStream toConfig(KspPart part) throws IOException, URISyntaxException {
		String config = new String(Files.readAllBytes(Paths.get(getClass().getClassLoader().getResource("part-template.cfg").toURI())));

		for (KspPartField f : KspPartField.values()) {
			if (!f.isOption()){
				config = config.replaceAll("%"+f.name()+"%", part.get(f));
			} else {
				if (part.get(f) != null) {
					config = config.replaceAll("%"+f.name()+"%", f.name()+" = "+part.get(f));
				} else {
					config = config.replaceAll("    %"+f.name()+"%\n", "");
				}
			}
		}
		config = config.replaceAll("%MODEL%", getBlockModel(part));
		config = config.replaceAll("%RESOURCE%\n", getBlockResource(part));
		config = config.replaceAll("%node_stack%", getBlockNodeStack(part));

		config = config.replaceAll("%explosionPotential%", "explosionPotential = "+part.get(KspPartField.explosionPotential));
		return new ByteArrayInputStream(config.getBytes(StandardCharsets.UTF_8));
	}

	private String getBlockModel(KspPart part) {
		List<String> res = new ArrayList<>();
		if (part.getModel() != null) {
			res.add("    MODEL");
			res.add("    {");
			res.add("        model = "+part.getModel().get(KspModelField.model));
			if (part.getModel().get(KspModelField.scale) != null) {
				res.add("        scale = "+part.getModel().get(KspModelField.scale));
			}
			if (part.getModel().get(KspModelField.texture) != null) {
				res.add("        texture = "+part.getModel().get(KspModelField.texture));
			}
			res.add("    }");
		}
		return res.stream().collect(Collectors.joining("\n"));
	}

	private String getBlockResource(KspPart part) {
		List<String> res = new ArrayList<>();
		if (part.getResources() != null) {
			for (KspResource resource : part.getResources()) {
				KspResourceType type = KspResourceType.valueOf(resource.get(KspResourceField.name));
				if (type != null && type.isGeneric()) {
					res.add("    RESOURCE");
					res.add("    {");
					res.add("        name = "+resource.get(KspResourceField.name));
					if (resource.get(KspResourceField.amount) != null) {
						res.add("        amount = "+resource.get(KspResourceField.amount));
					}
					if (resource.get(KspResourceField.maxAmount) != null) {
						res.add("        maxAmount = "+resource.get(KspResourceField.maxAmount));
					}
					res.add("    }");
				}
			}
			res.add("");
		}
		return res.stream().collect(Collectors.joining("\n"));
	}

	private String getBlockNodeStack(KspPart part) {
		List<String> res = new ArrayList<>();
		for (KspPartField f : KspPartField.values()) {
			if (f.name().startsWith("node_stack") && part.get(f) != null) {
				Arrays.stream(part.get(f).split("\n"))
					.forEach(line -> res.add("    "+f.name()+" = "+line));
			}
		}
		return res.stream().collect(Collectors.joining("\n"));
	}
}
