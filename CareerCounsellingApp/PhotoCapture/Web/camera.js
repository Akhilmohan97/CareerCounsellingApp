const video = document.getElementById("video");

async function startCamera() {

    const stream = await navigator.mediaDevices.getUserMedia({

        video: {

            facingMode: "environment"

        }

    });

    video.srcObject = stream;

}

startCamera();