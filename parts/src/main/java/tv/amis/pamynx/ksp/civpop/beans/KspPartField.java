package tv.amis.pamynx.ksp.civpop.beans;

public enum KspPartField implements KspConfigField {
	name,
	module,
	category,
	subcategory,
	author,
	scale(true),
	rescaleFactor(true),
	node_stack_top,
	node_stack_bottom,
	node_stack_front,
	node_stack_back,
	node_stack_left,
	node_stack_right,
	node_attach(true),
	CrewCapacity(true),
	TechRequired,
	entryCost,
	cost,
	title,
	manufacturer,
	description,
	attachRules,
	mass,
	dragModelType,
	maximum_drag,
	minimum_drag,
	angularDrag,
	crashTolerance,
	breakingForce(true),
	breakingTorque(true),
	maxTemp,
	explosionPotential(true),
	vesselType(true),
	;
	
	private boolean option;
	
	private KspPartField(boolean option) {
		this.option = option;
	}

	private KspPartField() {
		this(false);
	}

	public boolean isOption() {
		return option;
	}

}
