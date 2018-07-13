using System.Threading.Tasks;
using FinancialApi.Controllers;
using FinancialApi.Models.DTO.Request;  
using FinancialApi.Models.DTO.Response;
using FinancialApi.Services;
using FinancialApiUnitTests.Factory;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;

namespace FinancialApi.UnitTests.Controllers
{
    public class FinancialControllerTest
    {
        
        private FinancialController MockController(IBaseDTO payResult = null, IBaseDTO receiveResult = null){
            if (payResult == null)
                payResult = new OkDTO("x");

            if (receiveResult == null)
                receiveResult = new OkDTO("y");

            // Mock
            var mockPaymentService = new Mock<IPaymentService>();
            mockPaymentService.Setup(service => service.EnqueueToPay(It.IsAny<PaymentDTO>())).Returns(Task.FromResult<IBaseDTO>(payResult));

            // Mock
            var mockReceiptService = new Mock<IReceiptService>();
            mockReceiptService.Setup(service => service.EnqueueToReceive(It.IsAny<ReceiptDTO>())).Returns(Task.FromResult<IBaseDTO>(receiveResult));

            return new FinancialController(mockPaymentService.Object,
                                                     mockReceiptService.Object);
        }

        // Payment
        [Test]
        public async Task Pay_ReturnsAOkObjectResult_WithAUUID()
        {

            // initializaController
            var controller = MockController();

            // input (subject)
            var payment = PaymentFactory.Build();

            // test (result)
            var result = await controller.Payment(payment);

            // Assert one
            Assert.IsInstanceOf<OkObjectResult>(result);

            //Assert two
            var ResponseResult = (OkObjectResult)result;
            Assert.IsInstanceOf<OkDTO>(ResponseResult.Value);

            //Assert three
            var bodyResult = (OkDTO) ResponseResult.Value;
            Assert.AreEqual("x", bodyResult.UUID);
        }


        [Test]
        public async Task Pay_ReturnsABadRequestObjectResult_WithFailBody()
        {

            // initializaController
            var controller = MockController();
            controller.ModelState.AddModelError("Description", "some error");

            // input (subject)
            var payment = PaymentFactory.Build();

            // test (result)
            var result = await controller.Payment(payment);

            // Assert one
            Assert.IsInstanceOf<BadRequestObjectResult>(result);

            //Assert two
            var ResponseResult = (BadRequestObjectResult)result;
            Assert.IsInstanceOf<ErrorsDTO>(ResponseResult.Value);

            //Assert three
            var bodyResult = (ErrorsDTO)ResponseResult.Value;
            Assert.AreEqual(1, bodyResult.Details.Keys.Count);
            Assert.AreEqual("some error", bodyResult.Details["descricao"][0]);
        }

        //Receipt
        [Test]
        public async Task Receipt_ReturnsAOkObjectResult_WithAUUID()
        {

            // initializaController
            var controller = MockController();

            // input (subject)
            var receipt = ReceiptFactory.Build();

            // test (result)
            var result = await controller.Receipt(receipt);

            // Assert one
            Assert.IsInstanceOf<OkObjectResult>(result);

            //Assert two
            var ResponseResult = (OkObjectResult)result;
            Assert.IsInstanceOf<OkDTO>(ResponseResult.Value);
           
            //Assert three
            var bodyResult = (OkDTO)ResponseResult.Value;
        }
    }
}