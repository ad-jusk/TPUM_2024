using Commons;
using NJsonSchema;
using NJsonSchema.CodeGeneration.CSharp;

namespace ClassGenerator 
{

    class ClassGeneratorJson
    {
        private static void Main(string[] args)
        {
            JsonSchema instrumentSchema =  JsonSchema.FromType<InstrumentDTO>();
            JsonSchema newPriceSchema =  JsonSchema.FromType<NewPriceDTO>();

            instrumentSchema.Title = instrumentSchema.Title + "Generated";
            newPriceSchema.Title = newPriceSchema.Title + "Generated";

            saveCSharpFile(instrumentSchema, "../../../../Commons/InstrumentDTOGenerated.cs");
            saveCSharpFile(newPriceSchema, "../../../../Commons/NewPriceDTOGenerated.cs");
        }

        private static void saveCSharpFile(JsonSchema schema, string filename)
        {
            CSharpGenerator generator = new CSharpGenerator(schema, new CSharpGeneratorSettings()
            {
                Namespace = "GeneratedClasses",
                ClassStyle = CSharpClassStyle.Poco,
            });

            string code = generator.GenerateFile();

            File.WriteAllText(filename, code);
        }
    }
}