package tv.amis.pamynx.ksp.civpop.beans;

public enum KspResourceType {
	
	LiquidFuel,
	Oxidizer,
	SolidFuel,
	MonoPropellant,
	XenonGas,
	ElectricCharge,
	IntakeAir,
	//EVA Propellant,
	Ore,
	Ablator,
	Substrate(false),
	Water(false),
	EnrichedUranium(false),
	DepletedUranium(false),
	;

	public boolean generic;

	private KspResourceType(boolean generic) {
		this.generic = generic;
	}

	private KspResourceType() {
		this(true);
	}

	public boolean isGeneric() {
		return generic;
	}
}
