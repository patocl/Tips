using System;
using System.Reflection;
using CommandLine;
using CommandLine.Text;
using System.Collections.Generic;
using System.Security.Cryptography;

namespace PublicKeyTokenExample
{
    class Program
    {
        class Options
        {
            [Option('t', "target", Required = true, HelpText = "Type of target: snk or assembly.")]
            public string TargetType { get; set; }

            [Option('p', "path", Required = true, HelpText = "Path to the target SNK file or assembly.")]
            public string TargetPath { get; set; }
        }

        static void Main(string[] args)
        {
            Parser.Default.ParseArguments<Options>(args)
                .WithParsed(RetrievePublicKeyToken)
                .WithNotParsed(HandleParseError);
        }

        static void RetrievePublicKeyToken(Options options)
        {
            if (options.TargetType.ToLower() == "snk")
            {
                byte[] snkContent = System.IO.File.ReadAllBytes(options.TargetPath);
                byte[] publicKeyToken = GetPublicKeyTokenFromSNK(snkContent);
                DisplayPublicKeyToken(publicKeyToken);
            }
            else if (options.TargetType.ToLower() == "assembly")
            {
                Assembly assembly = Assembly.LoadFile(options.TargetPath);
                byte[] publicKeyToken = GetPublicKeyTokenFromAssembly(assembly);
                DisplayPublicKeyToken(publicKeyToken);
            }
            else
            {
                Console.WriteLine("Invalid target type. Use 'snk' or 'assembly'.");
            }
        }

        static byte[] GetPublicKeyTokenFromSNK(byte[] snk)
        {
            var snkp = new StrongNameKeyPair(snk);
            byte[] publicKey = snkp.PublicKey;

            using (var csp = new SHA1CryptoServiceProvider())
            {
                byte[] hash = csp.ComputeHash(publicKey);

                byte[] token = new byte[8];

                for (int i = 0; i < 8; i++)
                {
                    token[i] = hash[hash.Length - i - 1];
                }

                return token;
            }
        }

        static byte[] GetPublicKeyTokenFromAssembly(Assembly assembly)
        {
            return assembly.GetName().GetPublicKeyToken();
        }

        static void DisplayPublicKeyToken(byte[] publicKeyToken)
        {
            Console.WriteLine("Public Key Token:");
            Console.WriteLine(BitConverter.ToString(publicKeyToken).Replace("-", ""));
        }

        static void HandleParseError(IEnumerable<Error> errors)
        {
            // Handle command-line parsing errors
            Console.WriteLine("Error parsing command-line arguments.");
        }
    }
}
