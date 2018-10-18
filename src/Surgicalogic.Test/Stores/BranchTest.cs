using Moq;
using Xunit;

namespace Surgicalogic.Test.Stores
{
    using Surgicalogic.Contracts.Stores;
    using Surgicalogic.Model.CommonModel;
    using Surgicalogic.Model.EntityModel;
    using System.Threading.Tasks;

    public class BranchTest
    {
        [Fact]
        public async Task InsertBranch()
        {
            //Ekleyeceğimiz model.
            var branch = new BranchModel
            {
                Name = "Test",
                Description = "Test",
                IsActive = true,
            };

            //Geri dönmesini beklediğimiz sonuç
            var result = new ResultModel<BranchModel>()
            {
                Result = branch,
                Info = new Info()
            };

            //Store servis'i mockluyoruz.
            var mockbranchStoreService = new Mock<IBranchStoreService>();
            //Metodun geri döneceği sonucu ayarlıyoruz.
            mockbranchStoreService.Setup(x => x.InsertAndSaveAsync(branch)).ReturnsAsync(result);
            //Metodu çalıştırıyoruz.
            await mockbranchStoreService.Object.InsertAndSaveAsync(branch);
            //Metodun çalıştığını doğruluyoruz.
            mockbranchStoreService.Verify(x => x.InsertAndSaveAsync(branch), Times.Once());
        }

        [Fact]
        public async Task UpdateBranch()
        {
            //Güncelleyeceğimiz model
            var branch = new BranchModel
            {
                Id = 1,
                Name = "Test",
                Description = "Test",
                IsActive = true,
            };

            //Geri dönmesini beklediğimiz sonuç
            var result = new ResultModel<BranchModel>()
            {
                Result = branch,
                Info = new Info()
            };

            //Store servis'i mockluyoruz.
            var branchStoreService = new Mock<IBranchStoreService>();
            //Metodun geri döneceği sonucu ayarlıyoruz.
            branchStoreService.Setup(x => x.UpdateAndSaveAsync(branch)).ReturnsAsync(result);
            //Metodu çalıştırıyoruz.
            await branchStoreService.Object.UpdateAndSaveAsync(branch);
            //Metodun çalıştığını doğruluyoruz.
            branchStoreService.Verify(x => x.UpdateAndSaveAsync(branch), Times.Once());
        }

        [Fact]
        public async Task DeleteBranch()
        {
            //Sileceğimiz kayıt ID'si
            int branchId = 3;

            //Silme metodundan geri dönmesini beklediğimiz sonuç
            var deleteresult = new ResultModel<int>()
            {
                Result = branchId,
                Info = new Info()
            };

            //Sildiğimiz kaydı tekrar getirmek istediğimizde beklediğimiz sonuç.
            var findResult = new ResultModel<BranchModel>()
            {
                Result = null,
                Info = new Info()
            };

            //Store servis'i mockluyoruz.
            var mockpersonRepository = new Mock<IBranchStoreService>();

            //Silme metodunun geri döneceği sonucu ayarlayıp metodu çalıştırıyoruz.
            mockpersonRepository.Setup(x => x.DeleteAndSaveByIdAsync(branchId)).ReturnsAsync(deleteresult);
            await mockpersonRepository.Object.DeleteAndSaveByIdAsync(branchId);

            //ID'ye göre bulma metodunun geri döneceği sonucu ayarlayıp metodu çalıştırıyoruz.
            mockpersonRepository.Setup(x => x.FindByIdAsync(branchId)).ReturnsAsync(findResult);
            var t = (await mockpersonRepository.Object.FindByIdAsync(branchId));
            var q = t.Result;

            //FindByIdAsync metodunun null sonuç döndüğünü kontrol ediyoruz.
            Assert.Null(q);

            //Metodun çalıştığını doğruluyoruz.
            mockpersonRepository.Verify(x => x.DeleteAndSaveByIdAsync(branchId), Times.Once);
        }
    }
}
