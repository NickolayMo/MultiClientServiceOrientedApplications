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
        ReserveCarModelRules.push(new validator.PropertyRule('PickupDate',
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
        } else {
            viewModelHelper.modelErrors = $scope.reserveCarModel.errors;
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
    if (!$scope.reserveCarModel.initialized) {
        $location.path(CarRental+'customer/reservecar');
    }
    $scope.init = false;
    viewModelHelper.modelIsValid = true;
    viewModelHelper.modelErrors = [];
    $scope.viewMode = 'carlist';
    $scope.cars = [];
    $scope.reservationNumber = '';
    $scope.availableCars = function () {
        viewModelHelper.apiGet('api/reservation/availablecars/' + $scope.reserveCarModel.PickupDate + '/' + $scope.reserveCarModel.ReturnDate, null,
            function (result) {
                $scope.cars = result.cars;
                $scope.init = true;
            });
    }
    $scope.availableCars();
    $scope.selectCar = function (car) {
        var model = { PickupDate: car.PickupDate, ReturnDate: car.ReturnDate, Car: car.Id };
        viewModelHelper.apiPost('api/reservation/reservecar', model, function (result) {
            $scope.reservationNumber = result.data.ReservationId;
            $scope.viewMose = 'success';
        });
    }
});