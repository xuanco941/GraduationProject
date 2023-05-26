using GraduationProjectAPI.DataTransferObject;
using GraduationProjectAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace GraduationProjectAPI.Services.HistoryImageColorized
{
    public class HistoryImageColorizedService : IHistoryImageColorizedService
    {
        private readonly DatabaseContext _dbContext;

        public HistoryImageColorizedService(DatabaseContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<PaginatedListModel<Models.HistoryImageColorized>> Get(int pageNumber, int pageSize, int uid)
        {
            try
            {
                // Tính toán điểm bắt đầu và kết thúc
                int startRow = (pageNumber - 1) * pageSize;

                // Lấy tổng 
                int totalRows = await _dbContext.HistoryImageColorizeds.Where(a => a.UserID == uid).CountAsync();

                // Truy vấn theo khoảng cần phân trang
                var obj = await _dbContext.HistoryImageColorizeds.Where(a => a.UserID == uid).Skip(startRow).Take(pageSize).ToListAsync();

                // Trả về kết quả phân trang
                return new PaginatedListModel<Models.HistoryImageColorized>(obj, pageNumber, pageSize, totalRows);
            }
            catch
            {
                throw;
            }


        }

        public async Task<bool> Add(Models.HistoryImageColorized historyImage)
        {
            try
            {
                await _dbContext.HistoryImageColorizeds.AddAsync(historyImage);
                return await _dbContext.SaveChangesAsync() > 0;
            }
            catch
            {
                throw;
            }
        }
        public async Task<bool> AddRange(List<Models.HistoryImageColorized> historyImage)
        {
            try
            {
                await _dbContext.HistoryImageColorizeds.AddRangeAsync(historyImage);
                return await _dbContext.SaveChangesAsync() > 0;
            }
            catch
            {
                throw;
            }
        }

        public async Task<bool> Delete(List<int> ids)
        {
            try
            {
                var areas = await _dbContext.HistoryImageColorizeds.Where(a => ids.Contains(a.HistoryImageColorizedID)).ToListAsync();

                if (areas != null && areas.Any())
                {
                    _dbContext.HistoryImageColorizeds.RemoveRange(areas);
                }

                return await _dbContext.SaveChangesAsync() > 0;
            }
            catch
            {
                throw;
            }
        }




    }
}
