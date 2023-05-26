using GraduationProjectAPI.DataTransferObject;
using GraduationProjectAPI.Services.Context;
using GraduationProjectAPI.Services.HistoryImageColorized;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace GraduationProjectAPI.Controllers
{
    [ApiController]
    [Route("API/[controller]")]
    public class HistoryImageController
    {
        private readonly IHistoryImageColorizedService _historyImageColorizedService;
        private readonly IHttpContextMethod _httpContextMethod;


        public HistoryImageController(IHistoryImageColorizedService historyImageColorizedService, IHttpContextMethod httpContextMethod)
        {
            _historyImageColorizedService = historyImageColorizedService;
            _httpContextMethod = httpContextMethod;
        }

       
        [Authorize]
        [HttpGet("GetHistoryImageColorized")]
        public async Task<IActionResult> GetHistoryImageColorized(int pageNumber, int pageSize)
        {
            try
            {
                int uid = _httpContextMethod.GetIDContext();

                var result = await _historyImageColorizedService.Get(pageNumber,pageSize,uid);
                return new OkObjectResult(new APIResponse<PaginatedListModel<Models.HistoryImageColorized>>(result, "success", true));
            }
            catch
            {
                return new BadRequestResult();
            }

        }




        [Authorize]
        [HttpPost("DeleteHistoryImageColorized")]
        public async Task<IActionResult> DeleteHistoryImageColorized([FromBody] DeleteHistoryImageColorizedModel deleteHistory)
        {
            try
            {
                bool isDelete = await _historyImageColorizedService.Delete(deleteHistory.listID);
                if (isDelete == true)
                {
                    return new OkObjectResult(new APIResponse<bool>(isDelete, "Xóa thành công.", true));
                }
                else
                {
                    return new OkObjectResult(new APIResponse<bool>(false, "Xóa thất bại.", false));
                }

            }
            catch (Exception e)
            {
                return new OkObjectResult(new APIResponse<bool>(false, e.Message, false));
            }

        }
    }
}
