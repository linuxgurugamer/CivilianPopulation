package tv.amis.pamynx.ksp.civpop.beans;

public class KspModel extends KspConfig<KspModelField>{

	public KspModel() {
		super();
	}

	@Override
	public KspModelField[] values() {
		return KspModelField.values();
	}

	@Override
	public String getName() {
		return this.get(KspModelField.PART_name);
	}

}
