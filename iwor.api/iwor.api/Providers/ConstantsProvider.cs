using Microsoft.Extensions.Configuration;

namespace iwor.api.Providers
{
    public class ConstantsProvider : IConstantsProvider
    {
        public ConstantsProvider(IConfiguration config)
        {
            DateFormat = config.GetSection("Constants:DateFormat").Get<string>();
        }

        public string DateFormat { get; }
    }
}