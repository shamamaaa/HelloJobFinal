const range = document.querySelector('#ranger');
const intervals = document.querySelectorAll('.interval span');

const min = 0;
const max = 5;
const step = 1;

let currentValue = 0;
let currentPercent = 0;

range.addEventListener('mousedown', () => {
  document.addEventListener('mousemove', rangeMove);
});

document.addEventListener('mouseup', () => {
  document.removeEventListener('mousemove', rangeMove);
});

function rangeMove(event) {
  const rangeWidth = range.offsetWidth;
  const rangeLeft = range.getBoundingClientRect().left;
  let newValue = (event.clientX - rangeLeft) / rangeWidth * (max - min);

  if (newValue < min) {
    newValue = min;
  } else if (newValue > max) {
    newValue = max;
  }

  currentValue = Math.round(newValue / step) * step;
  currentPercent = ((currentValue - min) / (max - min)) * 100;

  document.querySelector('.range').style.width = `${currentPercent}%`;
  document.querySelectorAll('.handle').forEach((handle) => {
    handle.style.left = `${currentPercent}%`;
  });

  updateIntervals();
}

function updateIntervals() {
  intervals.forEach((interval) => {
    const intervalValue = parseInt(interval.innerText);
    if (currentValue < intervalValue) {
      interval.classList.add('inactive');
    } else {
      interval.classList.remove('inactive');
    }
  });
}

updateIntervals();
