using Entity;
using Microsoft.Extensions.DependencyInjection;

namespace DAO
{
    public static class IOCRegister
    {
        public static void InjectDAO(this IServiceCollection services)
        {
            services.AddTransient<IDAO<VB>, VBDAO>();
            services.AddTransient<IDAO<VB_SRC_TP>, VB_SRC_TPDAO>();
        }
    }
}
