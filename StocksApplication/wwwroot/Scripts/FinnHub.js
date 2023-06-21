const finnhubToken = document.getElementById("FinnhubToken").value;
const stockSymbol = document.getElementById("StockSymbol").value;
const socketUrl = `wss://ws.finnhub.io?token=${finnhubToken}`;
const stockPriceElem = document.getElementById("stock-price");

let ws;

// ApexCharts setup
let options = {
    chart: {
        type: 'line',
        height: 350,
        animations: {
            enabled: false
        }
    },
    series: [
        {
            name: "Price",
            data: []
        }
    ],
    xaxis: {
        type: 'datetime',
    },
    yaxis: {
        opposite: true,
        labels: {
            formatter: function (value) {
                return "$ " + value.toFixed(2);
            }
        }
    }
};

let chart = new ApexCharts(document.querySelector("#myChart"), options);
chart.render();

function connectWebSocket() {
    ws = new WebSocket(socketUrl);
    ws.onopen = function () {
        console.log("Finnhub WebSocket connected");
        ws.send(JSON.stringify({ type: "subscribe", symbol: stockSymbol }));
    };
    ws.onclose = function () {
        console.log("Finnhub WebSocket disconnected, attempting to reconnect in 5 seconds...");
        setTimeout(function () {
            connectWebSocket();
        }, 3000);
    };
    ws.onmessage = function (event) {
        const message = JSON.parse(event.data);
        if (message.data) {
            const stockPrice = message.data[0].p;
            stockPriceElem.innerText = stockPrice;
            document.getElementById("updated").value = stockPrice;

            let date = new Date();
            chart.appendData([{
                data: [{ x: date, y: stockPrice }]
            }]);
        }
    };
}

connectWebSocket();

setInterval(function () {
    if (ws.readyState === WebSocket.OPEN) {
        ws.send(JSON.stringify({ type: "subscribe", symbol: stockSymbol }));
    }
}, 3000);

window.addEventListener("beforeunload", function () {
    console.log("Closing Finnhub WebSocket...");
    ws.close();
});
