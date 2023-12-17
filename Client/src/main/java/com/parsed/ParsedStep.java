package com.parsed;

import com.soap.ws.client.generated.Step;

public class ParsedStep {
    private final String name;
    private final String instructions;

    public ParsedStep(Step step){
        name = step.getName().getValue();
        instructions = step.getInstruction().getValue();
    }

    public String getName(){
        return this.name;
    }

    @Override
    public String toString(){
        return String.format("%s (%s)", instructions, name);
    }
}
