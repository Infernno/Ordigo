using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Ordigo.Server.Core.Contracts;
using Ordigo.Server.Core.Data.Contexts;
using Ordigo.Server.Core.Data.Entities;

namespace Ordigo.Server.Core.Data.Repositories
{
    public class TextNoteRepository : GenericRepository<TextNote>, ITextNoteRepository
    {
        public TextNoteRepository(ApplicationDbContext mContext) : base(mContext)
        {
        }

        public override IEnumerable<TextNote> GetAll()
        {
            return Table.Include(n => n.Owner).AsNoTracking();
        }

        public override TextNote Find(Func<TextNote, bool> predicate)
        {
            return Table.Include(n => n.Owner).FirstOrDefault(predicate);
        }

        public override TextNote GetById(int id)
        {
            return Table.Include(n => n.Owner).First(e => e.Id == id);
        }
    }
}
