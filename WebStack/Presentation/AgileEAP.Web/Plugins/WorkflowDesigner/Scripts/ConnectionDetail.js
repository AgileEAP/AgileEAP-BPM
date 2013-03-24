function initConnection($scope) {
    $scope.processDefine = window.parent.$("#actionDialog").find("#bg_div_iframe")[0].contentWindow.processDefine;
    $scope.transition = function () {
        var transition = new Object();
        var processDefID = $.query.get("ProcessDefID");
        var transitionID = $.query.get("TransitionID");
        angular.forEach($scope.processDefine.Transitions, function (Transition) {
            if (processDefID == $scope.processDefine.ID && Transition.ID == transitionID) {
                transition = Transition;
            }
        });
        window.parent.$("#actionDialog").find("#bg_div_iframe")[0].contentWindow.setConnectionLabel(transition.Name);
        return transition;
    };
}
