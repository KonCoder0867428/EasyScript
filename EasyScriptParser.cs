using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

public class EasyScriptParser {
  private static readonly Regex _keywordRegex = new Regex(@"\b(if|else|for|while|switch|case|default|break|continue|return|function|class|extends|implements|try|catch|finally|throw|new|this|super|in|instanceof)\b");
  private static readonly Regex _identifierRegex = new Regex(@"[a-zA-Z_][a-zA-Z_0-9]*");
  private static readonly Regex _numberRegex = new Regex(@"\d+(\.\d+)?");
  private static readonly Regex _stringRegex = new Regex(@"""[^""]*""");
  private static readonly Regex _regexRegex = new Regex(@"/[^/]+/");

  public static AstNode Parse(string code) {
    var tokens = Tokenize(code);
    var ast = new AstNode();
    ParseTokens(tokens, ast);
    return ast;
  }

  private static List<Token> Tokenize(string code) {
      var tokens = new List<Token>();
      var matches = _keywordRegex.Matches(code);
      foreach (Match match in matches) {
        tokens.Add(new Token { Type = TokenType.Keyword, Value = match.Value });
      }
      matches = _identifierRegex.Matches(code);
      foreach (Match match in matches) {
        tokens.Add(new Token { Type = TokenType.Identifier, Value = match.Value });
      }
      matches = _numberRegex.Matches(code);
      foreach (Match match in matches) {
        tokens.Add(new Token { Type = TokenType.Number, Value = match.Value });
      }
      matches = _stringRegex.Matches(code);
      foreach (Match match in matches) {
        tokens.Add(new Token { Type = TokenType.String, Value = match.Value.Trim('"') });
      }
      matches = _regexRegex.Matches(code);
      foreach (Match match in matches) {
        tokens.Add(new Token { Type = TokenType.Regex, Value = match.Value.Trim('/') });
      }
      return tokens;
    }

    private static void ParseTokens(List<Token> tokens, AstNode ast) {
    var tokenIndex = 0;
    while (tokenIndex < tokens.Count) {
      var token = tokens[tokenIndex];
      switch (token.Type) {
        case TokenType.Keyword:
          switch (token.Value) {
            case "if":
              ast.Children.Add(ParseIfStatement(tokens, ref tokenIndex));
              break;
            case "else":
              ast.Children.Add(ParseElseStatement(tokens, ref tokenIndex));
              break;
            case "for":
              ast.Children.Add(ParseForLoop(tokens, ref tokenIndex));
              break;
            case "while":
              ast.Children.Add(ParseWhileLoop(tokens, ref tokenIndex));
              break;
            case "switch":
              ast.Children.Add(ParseSwitchStatement(tokens, ref tokenIndex));
              break;
            case "case":
              ast.Children.Add(ParseCaseStatement(tokens, ref tokenIndex));
              break;
            case "default":
              ast.Children.Add(ParseDefaultStatement(tokens, ref tokenIndex));
              break;
            case "break":
              ast.Children.Add(ParseBreakStatement(tokens, ref tokenIndex));
              break;
            case "continue":
              ast.Children.Add(ParseContinueStatement(tokens, ref tokenIndex));
              break;
            case "return":
              ast.Children.Add(ParseReturnStatement(tokens, ref tokenIndex));
              break;
            case "function":
              ast.Children.Add(ParseFunctionDeclaration(tokens, ref tokenIndex));
              break;
            case "class":
              ast.Children.Add(ParseClassDeclaration(tokens, ref tokenIndex));
              break;
            case "extends":
              ast.Children.Add(ParseExtendsStatement(tokens, ref tokenIndex));
              break;
            case "implements":
              ast.Children.Add(ParseImplementsStatement(tokens, ref tokenIndex));
              break;
            case "try":
              ast.Children.Add(ParseTryStatement(tokens, ref tokenIndex));
              break;
            case "catch":
              ast.Children.Add(ParseCatchStatement(tokens, ref tokenIndex));
              break;
            case "finally":
              ast.Children.Add(ParseFinallyStatement(tokens, ref tokenIndex));
              break;
            case "throw":
              ast.Children.Add(ParseThrowStatement(tokens, ref tokenIndex));
              break;
            case "new":
              ast.Children.Add(ParseNewExpression(tokens, ref tokenIndex));
              break;
            case "this":
              ast.Children.Add(ParseThisExpression(tokens, ref tokenIndex));
              break;
            case "super":
              ast.Children.Add(ParseSuperExpression(tokens, ref tokenIndex));
              break;
            case "in":
              ast.Children.Add(ParseInExpression(tokens, ref tokenIndex));
              break;
            case "instanceof":
              ast.Children.Add(ParseInstanceofExpression(tokens, ref tokenIndex));
              break;
          }
          break;
        case TokenType.Identifier:
          ast.Children.Add(ParseIdentifier(tokens, ref tokenIndex));
          break;
        case TokenType.Number:
          ast.Children.Add(ParseNumber(tokens, ref tokenIndex));
          break;
        case TokenType.String:
          ast.Children.Add(ParseString(tokens, ref tokenIndex));
          break;
        case TokenType.Regex:
          ast.Children.Add(ParseRegex(tokens, ref tokenIndex));
          break;
      }
      tokenIndex++;
    }
  }

  private static AstNode ParseIfStatement(List<Token> tokens, ref int tokenIndex) {
    var ifStatement = new AstNode { Type = "IfStatement" };
    tokenIndex++; // skip "if"
    ifStatement.Children.Add(ParseExpression(tokens, ref tokenIndex));
    tokenIndex++; // skip "("
    ifStatement.Children.Add(ParseStatement(tokens, ref tokenIndex));
    tokenIndex++; // skip ")"
    if (tokens[tokenIndex].Value == "else") {
      tokenIndex++;
      ifStatement.Children.Add(ParseStatement(tokens, ref tokenIndex));
    }
    return ifStatement;
  }

  private static AstNode ParseElseStatement(List<Token> tokens, ref int tokenIndex) {
    var elseStatement = new AstNode { Type = "ElseStatement" };
    tokenIndex++; // skip "else"
    elseStatement.Children.Add(ParseStatement(tokens, ref tokenIndex));
    return elseStatement;
  }

  private static AstNode ParseForLoop(List<Token> tokens, ref int tokenIndex) {
    var forLoop = new AstNode { Type = "ForLoop" };
    tokenIndex++; // skip "for"
    forLoop.Children.Add(ParseVariableDeclaration(tokens, ref tokenIndex));
    tokenIndex++; // skip "="
    forLoop.Children.Add(ParseExpression(tokens, ref tokenIndex));
    tokenIndex++; // skip ";"
    forLoop.Children.Add(ParseExpression(tokens, ref tokenIndex));
    tokenIndex++; // skip ";"
    forLoop.Children.Add(ParseStatement(tokens, ref tokenIndex));
    return forLoop;
  }

  private static AstNode ParseWhileLoop(List<Token> tokens, ref int tokenIndex) {
    var whileLoop = new AstNode { Type = "WhileLoop" };
    tokenIndex++; // skip "while"
    whileLoop.Children.Add(ParseExpression(tokens, ref tokenIndex));
    tokenIndex ++; // skip "("
    whileLoop.Children.Add(ParseStatement(tokens, ref tokenIndex));
    tokenIndex++; // skip ")"
    return whileLoop;
  }

  private static AstNode ParseSwitchStatement(List<Token> tokens, ref int tokenIndex) {
    var switchStatement = new AstNode { Type = "SwitchStatement" };
    tokenIndex++; // skip "switch"
    switchStatement.Children.Add(ParseExpression(tokens, ref tokenIndex));
    tokenIndex++; // skip "("
    while (tokens[tokenIndex].Value == "case") {
      tokenIndex++;
      switchStatement.Children.Add(ParseCaseStatement(tokens, ref tokenIndex));
    }
    if (tokens[tokenIndex].Value == "default") {
      tokenIndex++;
      switchStatement.Children.Add(ParseDefaultStatement(tokens, ref tokenIndex));
    }
    tokenIndex++; // skip ")"
    return switchStatement;
  }

  private static AstNode ParseCaseStatement(List<Token> tokens, ref int tokenIndex) {
    var caseStatement = new AstNode { Type = "CaseStatement" };
    tokenIndex++; // skip "case"
    caseStatement.Children.Add(ParseExpression(tokens, ref tokenIndex));
    tokenIndex++; // skip ":"
    caseStatement.Children.Add(ParseStatement(tokens, ref tokenIndex));
    return caseStatement;
  }

  private static AstNode ParseDefaultStatement(List<Token> tokens, ref int tokenIndex) {
    var defaultStatement = new AstNode { Type = "DefaultStatement" };
    tokenIndex++; // skip "default"
    tokenIndex++; // skip ":"
    defaultStatement.Children.Add(ParseStatement(tokens, ref tokenIndex));
    return defaultStatement;
  }

  private static AstNode ParseBreakStatement(List<Token> tokens, ref int tokenIndex) {
    var breakStatement = new AstNode { Type = "BreakStatement" };
    tokenIndex++; // skip "break"
    return breakStatement;
  }

  private static AstNode ParseContinueStatement(List<Token> tokens, ref int tokenIndex) {
    var continueStatement = new AstNode { Type = "ContinueStatement" };
    tokenIndex++; // skip "continue"
    return continueStatement;
  }

  private static AstNode ParseReturnStatement(List<Token> tokens, ref int tokenIndex) {
    var returnStatement = new AstNode { Type = "ReturnStatement" };
    tokenIndex++; // skip "return"
    returnStatement.Children.Add(ParseExpression(tokens, ref tokenIndex));
    return returnStatement;
  }

  private static AstNode ParseFunctionDeclaration(List<Token> tokens, ref int tokenIndex) {
    var functionDeclaration = new AstNode { Type = "FunctionDeclaration" };
    tokenIndex++; // skip "function"
    functionDeclaration.Children.Add(ParseIdentifier(tokens, ref tokenIndex));
    tokenIndex++; // skip "("
    while (tokens[tokenIndex].Type == TokenType.Identifier) {
      functionDeclaration.Children.Add(ParseIdentifier(tokens, ref tokenIndex));
      tokenIndex++; // skip ","
    }
    tokenIndex++; // skip ")"
    functionDeclaration.Children.Add(ParseStatement(tokens, ref tokenIndex));
    return functionDeclaration;
  }

  private static AstNode ParseClassDeclaration(List<Token> tokens, ref int tokenIndex) {
    var classDeclaration = new AstNode { Type = "ClassDeclaration" };
    tokenIndex++; // skip "class"
    classDeclaration.Children.Add(ParseIdentifier(tokens, ref tokenIndex));
    if (tokens[tokenIndex].Value == "extends") {
      tokenIndex++;
      classDeclaration.Children.Add(ParseIdentifier(tokens, ref tokenIndex));
    }
    if (tokens[tokenIndex].Value == "implements") {
      tokenIndex++;
      classDeclaration.Children.Add(ParseIdentifier(tokens, ref tokenIndex));
    }
    classDeclaration.Children.Add(ParseStatement(tokens, ref tokenIndex));
    return classDeclaration;
  }

  private static AstNode ParseExtendsStatement(List<Token> tokens, ref int tokenIndex) {
    var extendsStatement = new AstNode { Type = "ExtendsStatement" };
    tokenIndex++; // skip "extends"
    extendsStatement.Children.Add(ParseIdentifier(tokens, ref tokenIndex));
    return extendsStatement;
  }

  private static AstNode ParseImplementsStatement(List<Token> tokens, ref int tokenIndex) {
    var implementsStatement = new AstNode { Type = "ImplementsStatement" };
    tokenIndex++; // skip "implements"
    implementsStatement.Children.Add(ParseIdentifier(tokens, ref tokenIndex));
    return implementsStatement;
  }

  private static AstNode ParseTryStatement(List<Token> tokens, ref int tokenIndex) {
    var tryStatement = new AstNode { Type = "TryStatement" };
    tokenIndex++; // skip "try"
    tryStatement.Children.Add(ParseStatement(tokens, ref tokenIndex));
    if (tokens[tokenIndex].Value == "catch") {
      tokenIndex++;
      tryStatement.Children.Add(ParseCatchStatement(tokens, ref tokenIndex));
    }
    if (tokens[tokenIndex].Value == "finally") {
      tokenIndex++;
      tryStatement.Children.Add(ParseFinallyStatement(tokens, ref tokenIndex));
    }
    return tryStatement;
  }

  private static AstNode ParseCatchStatement(List<Token> tokens, ref int tokenIndex) {
    var catchStatement = new AstNode { Type = "CatchStatement" };
    tokenIndex++; // skip "catch"
    catchStatement.Children.Add(ParseIdentifier(tokens, ref tokenIndex));
    tokenIndex++; // skip "("
    catchStatement.Children.Add(ParseStatement(tokens, ref tokenIndex));
    tokenIndex++; // skip ")"
    return catchStatement;
  }

  private static AstNode ParseFinallyStatement(List<Token> tokens, ref int tokenIndex) {
    var finallyStatement = new AstNode { Type = "FinallyStatement" };
    tokenIndex++; // skip "finally"
    finallyStatement.Children.Add(ParseStatement(tokens, ref tokenIndex));
    return finallyStatement;
  }

  private static AstNode ParseThrowStatement(List<Token> tokens, ref int tokenIndex) {
    var throwStatement = new AstNode { Type = "ThrowStatement" };
    tokenIndex++; // skip "throw"
    throwStatement.Children.Add(ParseExpression(tokens, ref tokenIndex));
    return throwStatement;
  }

  private static AstNode ParseNewExpression(List<Token> tokens, ref int tokenIndex) {
    var newExpression = new AstNode { Type = "NewExpression" };
    tokenIndex++; // skip "new"
    newExpression.Children.Add(ParseIdentifier(tokens, ref tokenIndex));
    tokenIndex++; // skip "("
    while (tokens[tokenIndex].Type == TokenType.Identifier) {
      newExpression.Children.Add(ParseIdentifier(tokens, ref tokenIndex));
      tokenIndex++; // skip ","
    }
    tokenIndex++; // skip ")"
    return newExpression;
  }

  private static AstNode ParseThisExpression(List<Token> tokens, ref int tokenIndex) {
    var thisExpression = new AstNode { Type = "ThisExpression" };
    tokenIndex++; // skip "this"
    return thisExpression;
  }

  private static AstNode ParseSuperExpression(List<Token> tokens, ref int tokenIndex) {
    var superExpression = new AstNode { Type = "SuperExpression" };
    tokenIndex++; // skip "super"
    return superExpression;
  }

  private static AstNode ParseInExpression(List<Token> tokens, ref int tokenIndex) {
    var inExpression = new AstNode { Type = "InExpression" };
    tokenIndex++; // skip "in"
    inExpression.Children.Add(ParseExpression(tokens, ref tokenIndex));
    return inExpression;
  }

  private static AstNode ParseInstanceofExpression(List<Token> tokens, ref int tokenIndex) {
    var instanceofExpression = new AstNode { Type = "InstanceofExpression" };
    tokenIndex++; // skip "instanceof"
    instanceofExpression.Children.Add(ParseExpression(tokens, ref tokenIndex));
    return instanceofExpression;
  }

  private static AstNode ParseIdentifier(List<Token> tokens, ref int tokenIndex) {
    var identifier = new AstNode { Type = "Identifier" };
    identifier.Value = tokens[tokenIndex].Value;
    tokenIndex++;
    return identifier;
  }

  private static AstNode ParseNumber(List<Token> tokens, ref int tokenIndex) {
    var number = new AstNode { Type = "Number" };
    number.Value = tokens[tokenIndex].Value;
    tokenIndex++;
    return number;
  }

  private static AstNode ParseString(List<Token> tokens, ref int tokenIndex) {
    var str = new AstNode { Type = "String" };
    str.Value = tokens[tokenIndex].Value.Trim('"');
    tokenIndex++;
    return str;
  }

  private static AstNode ParseRegex(List<Token> tokens, ref int tokenIndex) {
    var regex = new AstNode { Type = "Regex" };
    regex.Value = tokens[tokenIndex].Value.Trim('/');
    tokenIndex++;
    return regex;
  }

  private static AstNode ParseExpression(List<Token> tokens, ref int tokenIndex) {
    var expression = new AstNode { Type = "Expression" };
    while (tokenIndex < tokens.Count) {
      switch (tokens[tokenIndex].Type) {
        case TokenType.Identifier:
          expression.Children.Add(ParseIdentifier(tokens, ref tokenIndex));
          break;
        case TokenType.Number:
          expression.Children.Add(ParseNumber(tokens, ref tokenIndex));
          break;
        case TokenType.String:
          expression.Children.Add(ParseString(tokens, ref tokenIndex));
          break;
        case TokenType.Regex:
          expression.Children.Add(ParseRegex(tokens, ref tokenIndex));
          break;
        default:
          tokenIndex++;
          break;
      }
    }
    return expression;
  }

  private static AstNode ParseStatement(List<Token> tokens, ref int tokenIndex) {
    var statement = new AstNode { Type = "Statement" };
    while (tokenIndex < tokens.Count) {
      switch (tokens[tokenIndex].Type) {
        case TokenType.Keyword:
          switch (tokens[tokenIndex].Value) {
            case "if":
              statement.Children.Add(ParseIfStatement(tokens, ref tokenIndex));
              break;
            case "else":
              statement.Children.Add(ParseElseStatement(tokens, ref tokenIndex));
              break;
            case "for":
              statement.Children.Add(ParseForLoop(tokens, ref tokenIndex));
              break;
            case "while":
              statement .Children.Add(ParseWhileLoop(tokens, ref tokenIndex));
              break;
            case "switch":
              statement.Children.Add(ParseSwitchStatement(tokens, ref tokenIndex));
              break;
            case "case":
              statement.Children.Add(ParseCaseStatement(tokens, ref tokenIndex));
              break;
            case "default":
              statement.Children.Add(ParseDefaultStatement(tokens, ref tokenIndex));
              break;
            case "break":
              statement.Children.Add(ParseBreakStatement(tokens, ref tokenIndex));
              break;
            case "continue":
              statement.Children.Add(ParseContinueStatement(tokens, ref tokenIndex));
              break;
            case "return":
              statement.Children.Add(ParseReturnStatement(tokens, ref tokenIndex));
              break;
            case "function":
              statement.Children.Add(ParseFunctionDeclaration(tokens, ref tokenIndex));
              break;
            case "class":
              statement.Children.Add(ParseClassDeclaration(tokens, ref tokenIndex));
              break;
            case "extends":
              statement.Children.Add(ParseExtendsStatement(tokens, ref tokenIndex));
              break;
            case "implements":
              statement.Children.Add(ParseImplementsStatement(tokens, ref tokenIndex));
              break;
            case "try":
              statement.Children.Add(ParseTryStatement(tokens, ref tokenIndex));
              break;
            case "catch":
              statement.Children.Add(ParseCatchStatement(tokens, ref tokenIndex));
              break;
            case "finally":
              statement.Children.Add(ParseFinallyStatement(tokens, ref tokenIndex));
              break;
            case "throw":
              statement.Children.Add(ParseThrowStatement(tokens, ref tokenIndex));
              break;
            case "new":
              statement.Children.Add(ParseNewExpression(tokens, ref tokenIndex));
              break;
            case "this":
              statement.Children.Add(ParseThisExpression(tokens, ref tokenIndex));
              break;
            case "super":
              statement.Children.Add(ParseSuperExpression(tokens, ref tokenIndex));
              break;
            case "in":
              statement.Children.Add(ParseInExpression(tokens, ref tokenIndex));
              break;
            case "instanceof":
              statement.Children.Add(ParseInstanceofExpression(tokens, ref tokenIndex));
              break;
          }
          break;
        case TokenType.Identifier:
          statement.Children.Add(ParseIdentifier(tokens, ref tokenIndex));
          break;
        case TokenType.Number:
          statement.Children.Add(ParseNumber(tokens, ref tokenIndex));
          break;
        case TokenType.String:
          statement.Children.Add(ParseString(tokens, ref tokenIndex));
          break;
        case TokenType.Regex:
          statement.Children.Add(ParseRegex(tokens, ref tokenIndex));
          break;
        default:
          tokenIndex++;
          break;
      }
    }
    return statement;
  }
}

public enum TokenType {
  Keyword,
  Identifier,
  Number,
  String,
  Regex
}

public class Token {
  public TokenType Type { get; set; }
  public string Value { get; set; }
}

public class AstNode {
  public string Type { get; set; }
  public string Value { get; set; }
  public List<AstNode> Children { get; set; }
}