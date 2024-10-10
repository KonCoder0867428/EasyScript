using System;
using System.Collections.Generic;

public class EasyScriptInterpreter {
  private readonly Dictionary<string, object> _variables = new Dictionary<string, object>();

  public void Execute(AstNode ast) {
    ExecuteNode(ast);
  }

  private void ExecuteNode(AstNode node) {
    switch (node.Type) {
      case "VariableDeclaration":
        DeclareVariable(node);
        break;
      case "Assignment":
        AssignVariable(node);
        break;
      case "IfStatement":
        ExecuteIfStatement(node);
        break;
      case "ForLoop":
        ExecuteForLoop(node);
        break;
      case "FunctionCall":
        ExecuteFunctionCall(node);
        break;
      default:
        throw new Exception("Unknown node type");
    }
  }

  private void DeclareVariable(AstNode node) {
    var variableName = node.Children[0].Value;
    _variables[variableName] = null;
  }

  private void AssignVariable(AstNode node) {
    var variableName = node.Children[0].Value;
    var value = EvaluateExpression(node.Children[1]);
    _variables[variableName] = value;
  }

  private object EvaluateExpression(AstNode node) {
    // Implement the expression evaluation logic here
    // ...
  }

  private void ExecuteIfStatement(AstNode node) {
    var condition = EvaluateExpression(node.Children[0]);
    if ((bool)condition) {
      ExecuteNode(node.Children[1]);
    } else if (node.Children.Count > 2) {
      ExecuteNode(node.Children[2]);
    }
  }

  private void ExecuteForLoop(AstNode node) {
    var variableName = node.Children[0].Value;
    var startValue = EvaluateExpression(node.Children[1]);
    var endValue = EvaluateExpression(node.Children[2]);
    for (var i = (int)startValue; i <= (int)endValue; i++) {
      _variables[variableName] = i;
      ExecuteNode(node.Children[3]);
    }
  }

  private void ExecuteFunctionCall(AstNode node) {
  var functionName = node.Children[0].Value;
  var arguments = new List<object>();
  foreach (var argumentNode in node.Children[1].Children) {
    arguments.Add(EvaluateExpression(argumentNode));
  }

  // Check if the function is a built-in function
  if (functionName == "print") {
    PrintFunction(arguments);
  } else if (functionName == "len") {
    LenFunction(arguments);
  } else if (functionName == "type") {
    TypeFunction(arguments);
  } else {
    // Check if the function is a user-defined function
    if (_variables.ContainsKey(functionName)) {
      var function = _variables[functionName] as AstNode;
      if (function != null) {
        // Create a new scope for the function call
        var scope = new Dictionary<string, object>(_variables);
        // Bind the function arguments to the scope
        for (int i = 0; i < arguments.Count; i++) {
          scope[function.Children[1].Children[i].Value] = arguments[i];
        }
        // Execute the function body
        ExecuteNode(function.Children[2], scope);
      } else {
        throw new Exception("Function not found");
      }
    } else {
      throw new Exception("Function not found");
    }
  }
}

private void PrintFunction(List<object> arguments) {
  Console.WriteLine(string.Join(" ", arguments));
}

private void LenFunction(List<object> arguments) {
  if (arguments.Count != 1) {
    throw new Exception("Invalid number of arguments");
  }
  var value = arguments[0];
  if (value is string) {
    Console.WriteLine(((string)value).Length);
  } else if (value is List<object>) {
    Console.WriteLine(((List<object>)value).Count);
  } else {
    throw new Exception("Invalid argument type");
  }
}

private void TypeFunction(List<object> arguments) {
  if (arguments.Count != 1) {
    throw new Exception("Invalid number of arguments");
  }
  var value = arguments[0];
  Console.WriteLine(value.GetType().Name);
}

private void ExecuteNode(AstNode node, Dictionary<string, object> scope) {
  switch (node.Type) {
    case "VariableDeclaration":
      DeclareVariable(node, scope);
      break;
    case "Assignment":
      AssignVariable(node, scope);
      break;
    case "IfStatement":
      ExecuteIfStatement(node, scope);
      break;
    case "ForLoop":
      ExecuteForLoop(node, scope);
      break;
    case "FunctionCall":
      ExecuteFunctionCall(node, scope);
      break;
    default:
      throw new Exception("Unknown node type");
  }
}

private void DeclareVariable(AstNode node, Dictionary<string, object> scope) {
  var variableName = node.Children[0].Value;
  scope[variableName] = null;
}

private void AssignVariable(AstNode node, Dictionary<string, object> scope) {
  var variableName = node.Children[0].Value;
  var value = EvaluateExpression(node.Children[1], scope);
  scope[variableName] = value;
}

private object EvaluateExpression(AstNode node, Dictionary<string, object> scope) {
  // Implement the expression evaluation logic here
  // ...
}

private void ExecuteIfStatement(AstNode node, Dictionary<string, object> scope) {
  var condition = EvaluateExpression(node.Children[0], scope);
  if ((bool)condition) {
    ExecuteNode(node.Children[1], scope);
  } else if (node.Children.Count > 2) {
    ExecuteNode(node.Children[2], scope);
  }
}

private void ExecuteForLoop(AstNode node, Dictionary<string, object> scope) {
  var variableName = node.Children[0].Value;
  var startValue = EvaluateExpression(node.Children[1], scope);
  var endValue = EvaluateExpression(node.Children[2], scope);
  for (var i = (int)startValue; i <= (int)endValue; i++) {
    scope[variableName] = i;
    ExecuteNode(node.Children[3], scope);
  }
}

private void ExecuteFunctionCall(AstNode node, Dictionary<string, object> scope) {
  var functionName = node.Children[0].Value;
  var arguments = new List<object>();
  foreach (var argumentNode in node.Children[1].Children) {
    arguments.Add(EvaluateExpression(argumentNode, scope));
  }
  // Implement the function call logic here
  // ...
}
}