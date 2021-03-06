﻿using System;

namespace Compiler_Project_Tanks___Carl_Meyer
{
    public class Token
    {
        String Spelling;
        int Type;
        // 1 Identifier, 
        // 2 Operator, 
        // 3 LPar, 
        // 4 RPar
        // 5 Literal
        // 6 If
        // 7 Then
        // 8 Else
        // 9 Let
        // 10 In 
        // 11 End 
        // Colon = 12;
        // Var = 13;
        // SemiColon = 14;
        public Token(String S, int T)
        {
            Spelling = S;
            Type = T;
        }

        public String getSpelling()
        {
            return Spelling;
        }

        public int getType()
        {
            return Type;
        }

        public void showSpelling()
        {
            Console.WriteLine(Spelling);
        }

        public Boolean matches(String other)
        {
            return (this.Spelling.Equals(other));
        }

        public Boolean matchesType(int T)
        {
            return (Type == T);
        }

    }
}