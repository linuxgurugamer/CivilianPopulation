package tv.amis.pamynx.ksp.civpop.beans;

public class KspPart extends KspConfig<KspPartField>{

	public KspPart() {
		super();
	}
	
	@Override
	public KspPartField[] values() {
		return KspPartField.values();
	}

	@Override
	public String getName() {
		return this.get(KspPartField.name);
	}

	public void setName(String name) {
		this.set(KspPartField.name, name);
	}

	public String getCategory() {
		return this.get(KspPartField.category);
	}

	public void setCategory(String category) {
		this.set(KspPartField.category, category);
	}
}
