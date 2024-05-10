using System.Reflection;
using TopazVideoLab.Project;

namespace TopazVideoLab2.Models
{
    public class ModelManager : IModelManager
    {
        private static Type[] ModelTypes;
        private static ModelBase[] Models;

        static ModelManager()
        {
            ModelTypes = Assembly.GetAssembly(typeof(ModelManager)).GetTypes().Where(t => t.IsSubclassOf(typeof(ModelBase))).ToArray();
            Models = ModelTypes.Select(t =>(ModelBase)Activator.CreateInstance(t)).ToArray();
        }

        public static (UpscaleAlgorithm UpscaleAlgorithm, int DefaultFactor)[] Templates
        {
            get
            {
                return new []
                {
                    UpscaleAlgorithm.GaiaHq,
                    UpscaleAlgorithm.Proteus4,
                    UpscaleAlgorithm.ArtemisHq,
                    UpscaleAlgorithm.IrisMq,
                    UpscaleAlgorithm.Dione,
                    UpscaleAlgorithm.Themis2,
                    UpscaleAlgorithm.Nyx2,
                }.Select(a =>
                {
                    var model = GetModel(a);
                    return (model.UpscaleAlgorithm, model.DefaultFactor);
                }).ToArray();
            }
        }

        public static UpscaleAlgorithm[] Order => new[]
        {
            UpscaleAlgorithm.GaiaHq,
            UpscaleAlgorithm.GaiaCg,
            UpscaleAlgorithm.Dione,
            UpscaleAlgorithm.Proteus,
            UpscaleAlgorithm.Proteus4,
            UpscaleAlgorithm.IrisMq,
            UpscaleAlgorithm.IrisLq3,
            UpscaleAlgorithm.Iris,
            UpscaleAlgorithm.ArtemisHq,
            UpscaleAlgorithm.ArtemisMq,
            UpscaleAlgorithm.ArtemisLq,
            UpscaleAlgorithm.Themis2,
            UpscaleAlgorithm.Nyx2,
        };

        public static int GetOrder(UpscaleAlgorithm algorithm)
        {
            return Order.ToList().IndexOf(algorithm);
        }

        public static UpscaleAlgorithm GetAlgorithm(int order)
        {
            return Order[order];
        }

        public static string[] Names => Order.Select(a => GetModel(a).Name).ToArray();

        public static ModelBase GetModel(UpscaleAlgorithm algorithm)
        {
            return Models.FirstOrDefault(m => m.UpscaleAlgorithm == algorithm);
        }

        IModel IModelManager.GetModel(UpscaleAlgorithm algorithm)
        {
            return GetModel(algorithm);
        }

        public static ModelBase GetModel(string topazName)
        {
            return Models.FirstOrDefault(m => m.TopazName == topazName);
        }
    }
}
