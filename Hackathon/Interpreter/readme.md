# Maintaining Interpreter

The interpreter is created via VCC, therefore you must put all references to own classes in comments, so that VCC is able to compile your compiler/ interpreter.

After creating the compiler/ interpreter with VCC, you have to:
1. move the C# file to project
1. change namespace to **Hackathon.Interpreter;**
2. take the references out of the comments
3. add filenames for the inputfile (*InputFilename*) and the outputfile (*OutputFilename*)
