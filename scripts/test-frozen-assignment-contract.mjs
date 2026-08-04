import assert from "node:assert/strict";
import { readFileSync } from "node:fs";

const source = readFileSync("Runtime/Scripts/Dto/RemoteConfigurationSource.cs", "utf8");
const dependencies = readFileSync("Editor/QonversionDependencies.xml", "utf8");

assert.match(source, /type\s*==\s*"frozen"[\s\S]*RemoteConfigurationAssignmentType\.Frozen/);
assert.match(source, /return\s+RemoteConfigurationAssignmentType\.Unknown/);
assert.match(source, /enum\s+RemoteConfigurationAssignmentType[\s\S]*Frozen/);
assert.match(dependencies, /io\.qonversion:sandwich:7\.13\.0/);
assert.match(dependencies, /QonversionSandwich" version="7\.13\.0"/);

console.log("Frozen assignment bridge contract is intact.");
