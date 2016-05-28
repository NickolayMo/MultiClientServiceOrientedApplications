var accountRegisterModule = angular.module('accountRegister',['common']);

accountRegisterModule.controller('AccountRegisterViewModel', function($scope, $http, $location, $window, viewModelHelper){
    $scope.viewModelHelper = viewModelHelper;
    $scope.accountModelStep1 = new CarRental.AccountRegisterModelStep1();
    $scope.accountModelStep2 = new CarRental.AccountRegisterModelStep2();
    $scope.accountModelStep3 = new CarRental.AccountRegisterModelStep3();
    
    $scope.previous = function(){
        $window.history.back();
    }

});