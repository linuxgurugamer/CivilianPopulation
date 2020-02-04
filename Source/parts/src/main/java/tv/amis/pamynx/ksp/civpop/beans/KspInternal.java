package tv.amis.pamynx.ksp.civpop.beans;

public class KspInternal extends KspConfig<KspInternalField>{

	public KspInternal() {
		super();
	}

	@Override
	public KspInternalField[] values() {
		return KspInternalField.values();
	}

	@Override
	public String getName() {
		return this.get(KspInternalField.PART_name);
	}

}
