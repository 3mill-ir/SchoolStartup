function Init(PlaceToDraw, ScoreResult, LableResult, BackGroundColors, BorderColors,lable,Type) {
    var ctx = document.getElementById(PlaceToDraw).getContext('2d');
    var myChart = new Chart(ctx, {
        type: Type,
        data: {
            labels: LableResult,
            datasets: [{
                label: lable,
                data: ScoreResult,
                backgroundColor: BackGroundColors,
                borderColor: BorderColors,
                borderWidth: 1
            }]
        },
        options: {
            scales: {
                yAxes: [{
                    ticks: {
                        beginAtZero: true
                    }
                }]
            }
        }
    });
}