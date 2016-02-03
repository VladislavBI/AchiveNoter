using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AchiveNoter
{
    class DelegatesData
    {
        /// <summary>
        /// Обновление внешнего вида ComboBox тем и подтем (AddingAchievement)
        /// </summary>
        public delegate void AchAddDelCBRefresf();

        /// <summary>
        /// Экземпляр делегата - 
        /// Обновление внешнего вида ComboBox тем и подтем (AddingAchievement) 
        /// </summary>
        public static AchAddDelCBRefresf HandlerAchAddDelCBRefresf;
    }
}
