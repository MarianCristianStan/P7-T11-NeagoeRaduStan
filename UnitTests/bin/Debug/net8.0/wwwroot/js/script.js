
function submitConfiguration() {
   var vehicleFeatures = [];
   document.querySelectorAll('.vehicle-feature').forEach(function (element) {
      var vehicleVIN = element.dataset.vin;
      var featureId = element.dataset.featureId;
      var value = element.querySelector('.feature-value').value;

      var feature = {
         VehicleVIN: parseInt(vehicleVIN),
         IdFeature: parseInt(featureId),
         SomeProperty: value W
      };

      vehicleFeatures.push(feature);
   });

   $.ajax({
      url: '/Configuration/UpdateConfiguration',
      type: 'POST',
      contentType: 'application/json',
      data: JSON.stringify(vehicleFeatures),
      success: function (response) {
         console.log('Configuration updated successfully');
         window.location.href = '/Home/Index';
      },
      error: function (xhr, status, error) {
         console.error('Error updating configuration:', error);
         window.location.href = '/Home/Index';
      }
   });
}
