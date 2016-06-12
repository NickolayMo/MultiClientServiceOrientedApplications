var reserveCarModule = angular.module('reserveCar', ['common'])
.config(function($routeProvider, $locationProvider){
    $routeProvider.when(CarRental.rootPath + 'customer/reservecar', {templateUrl: CarRental.rootPath + 'Templates/Reserve.html', controller:'ReserveViewModel'});
    $routeProvider.when(CarRental.rootPath + 'customer/reservecar/carlist',{templateUrl: CarRental.rootPath + 'Templates/CarList.html', controller: 'CarListViewModel'});
    $routeProvider.otherwise({redirectTo: CarRental.rootPath +'customer/reservecar'});
    $locationProvider.html5Mode(true);
});

reserveCarModule.controller("ReserveCarViewModel", function ($scope, $window, viewModelHelper) {

    $scope.viewModelHelper = viewModelHelper;
    $scope.reserveCarModel = new CarRental.ReserveCarModel();

    $scope.previous = function () {
        $window.history.back();
    }
});

reserveCarModule.controller('ReserveViewModel', function ($scope, $http, $location, $window, viewModelHelper, validator) {
    viewModelHelper.modelIsValid = true;
    viewModelHelper.modelErrors = [];
    
    var ReserveCarModelRules = [];
    var setupRules = function () {
        ReserveCarModelRules.push(new validator.PropertyRule('PickUpdate',
           {
               required: { message: "PickUp date is required" }
           }));
        ReserveCarModelRules.push(new validator.PropertyRule('ReturnDate',
           {
               required: { message: "Return date is required" }
           }));
    }
    $scope.submit = function () {
        if ($scope.reserveCarModel.PickupDate !== null && $scope.reserveCarModel.PickupDate !=='') {
            $scope.reserveCarModel.PickupDate = moment($scope.reserveCarModel.PickupDate).format("MM-DD-YYYY");
        }
        else {
            $scope.reserveCarModel.PickupDate = '';
        }
        if ($scope.reserveCarModel.ReturnDate !== null && $scope.reserveCarModel.ReturnDate !=='') {
            $scope.reserveCarModel.ReturnDate = moment($scope.reserveCarModel.ReturnDate).format("MM-DD-YYYY");
        }
        else {
            $scope.reserveCarModel.ReturnDate = '';
        }
        validator.ValidateModel($scope.reserveCarModel, ReserveCarModelRules);
        viewModelHelper.modelIsValid = $scope.reserveCarModel.isValid;
        viewModelHelper.modelErrors = $scope.reserveCarModel.errors;
        if (viewModelHelper.modelIsValid) {
            $scope.reserveCarModel.initialized = true;
            $location.path(CarRental.rootPath + 'customer/reservecar/carlist');
        }

    }
    $scope.openPickup = function($event){
        $event.preventDefault();
        $event.stopPropagation();
        $scope.openedPickup = true;
    };
    $scope.openReturn = function ($event) {
         $event.preventDefault();
        $event.stopPropagation();
        $scope.openedReturn = true;
    };

    setupRules();

});

reserveCarModule.controller('CarListViewModel', function ($scope, $http, $location, $window, viewModelHelper, validator) { 
    viewModelHelper.modelIsValid = true;
    viewModelHelper.modelErrors = [];
});