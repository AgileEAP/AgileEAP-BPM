function Configure($scope) {
    var processDefID = $.query.get("ProcessDefID");
    var activityID = $.query.get("ActivityID");
    $scope = window.parent.$("#actionDialog").find("#bg_div_iframe")[0].contentWindow.processDefine;
    $scope.basic = function () {
        angular.forEach($scope.Activities, function (Activitie) {
            if (processDefID == $scope.ID && Activitie.ID == activityID) {
                return Activitie;
            }
        });
    }
}
