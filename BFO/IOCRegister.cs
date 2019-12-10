using Entity;
using Microsoft.Extensions.DependencyInjection;

namespace BFO
{
    public static class IOCRegister
    {
        public static void InjectBFO(this IServiceCollection services)
        {
            services.AddTransient<BaseBFO<VB>, VBBFO>();
            services.AddTransient<BaseBFO<VB_SRC_TP>, VB_SRC_TPBFO>();
        }
    }
}
