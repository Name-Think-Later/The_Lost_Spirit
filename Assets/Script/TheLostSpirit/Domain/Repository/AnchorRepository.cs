using System;
using System.Collections.Generic;
using System.Linq;
using TheLostSpirit.Domain.Skill.Anchor;
using TheLostSpirit.Identity.EntityID;

namespace TheLostSpirit.Domain.Repository
{
    public class AnchorRepository : GenericRepository<AnchorID, AnchorEntity>
    {
        public List<AnchorEntity> GetManyByFormulaStream(Guid formulaStreamId) {
            return this.Where(entity => entity.FormulaStreamID == formulaStreamId).ToList();
        }
    }
}