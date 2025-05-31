using UPSWSWebAPIs.BAL.Interfaces;
using UPSWSWebAPIs.DAL.Interfaces;

namespace UPSWSWebAPIs.BAL.Implementations
{
    public class MasterBAL:IMasterBAL
    {
        public readonly IMasterDAL _MasterDAL;
        public MasterBAL(IMasterDAL MasterDAL)
        {
            _MasterDAL = MasterDAL;
        }
        public Task<string> GetDistricts()
        {
            return _MasterDAL.GetDistricts();
        }
    }
}
