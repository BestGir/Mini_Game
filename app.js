const appCuttingGame = (containerId, params = {}) => {
    // Параметры по умолчанию
    params.cellSize ||= 50;
    params.colCount ||= 4;
    params.rowCount ||= 4;
    params.lineWidth ||= 1;
    params.cellEmptyColor ||= "white";
    params.cellDeffColor ||= "gray";
    params.countShapes ||= 3;
    params.shapesColors ||= ["red", "green", "blue", "purple"];
    params.buttonList ||= [];
    params.gridColor ||= "black";
    params.countCellsInShape ||= 4;

    const loadCSS = (cssFile) => {
        const link = document.createElement("link");
        link.rel = "stylesheet";
        link.href = cssFile;
        document.head.appendChild(link);
    };

    if (params.css) {
        loadCSS(params.css);
    }

    let shapeColor = 0;
    let filling = false;
    let isViewingAnswer = false;
    let correctAnswersInRow = 0;

    const container = document.getElementById(containerId);
    if (!container) {
        alert(`Container with id "${containerId}" not found.`);
        return;
    }

    const wrapper = document.createElement("div");
    wrapper.classList.add("appCuttingGame");
    container.append(wrapper);

    const storageKey = `cuttingGame_${containerId}`;

    const saveGameState = () => {
        const gameState = {
            figure: figure,
            answerFigures: answerFigures,
            correctAnswersInRow: correctAnswersInRow,
            params: params,
            shapeColor: shapeColor,
            isViewingAnswer: isViewingAnswer,
        };
        localStorage.setItem(storageKey, JSON.stringify(gameState));
    };

    const loadGameState = () => {
        const savedState = localStorage.getItem(storageKey);
        if (savedState) {
            const gameState = JSON.parse(savedState);
            figure = gameState.figure;
            answerFigures = gameState.answerFigures;
            correctAnswersInRow = gameState.correctAnswersInRow;
            params = gameState.params;
            shapeColor = gameState.shapeColor;
            isViewingAnswer = gameState.isViewingAnswer;
        }
    };

    const prepareEmptyField = () => {
        const field = new Array(params.rowCount);
        for (let i = 0; i < params.rowCount; i++) {
            field[i] = new Array(params.colCount).fill(-1);
        }
        return field;
    };

    let figure = prepareEmptyField();
    let answerFigures = [];

    loadGameState();

    const canvas = document.createElement("canvas");
    canvas.classList.add("game-canvas");
    const context = canvas.getContext("2d");

    const canvasOnClick = (e) => {
        if (!filling || isViewingAnswer) return;
        const x = e.offsetX;
        const y = e.offsetY;
        const cellX = Math.floor(x / params.cellSize);
        const cellY = Math.floor(y / params.cellSize);
        if (figure[cellY] && figure[cellY][cellX] !== -1) {
            figure[cellY][cellX] = shapeColor;
            showCell(cellX, cellY, figure[cellY][cellX], context);
            saveGameState();
        }
    };

    let currentCellX = -1;
    let currentCellY = -1;

    const canvasOnMouseMove = (e) => {
        if (!filling || isViewingAnswer) return;
        const x = e.offsetX;
        const y = e.offsetY;
        const cellX = Math.floor(x / params.cellSize);
        const cellY = Math.floor(y / params.cellSize);
        if ((currentCellX !== cellX || currentCellY !== cellY) && figure[cellY] && figure[cellY][cellX] !== -1) {
            figure[cellY][cellX] = shapeColor;
            showCell(cellX, cellY, figure[cellY][cellX], context);
            currentCellX = cellX;
            currentCellY = cellY;
            saveGameState();
        }
    };

    const canvasOnMouseDown = (e) => {
        if (isViewingAnswer) return;
        filling = true;
        canvas.addEventListener("mousemove", canvasOnMouseMove);
    };

    const canvasOnMouseUp = (e) => {
        filling = false;
        canvas.removeEventListener("mousemove", canvasOnMouseMove);
    };

    canvas.addEventListener("mousedown", canvasOnMouseDown);
    canvas.addEventListener("mouseup", canvasOnMouseUp);
    canvas.addEventListener("click", canvasOnClick);

    const showCell = (x, y, color, ctx) => {
        const coordX = x * params.cellSize + params.lineWidth / 2 + 1;
        const coordY = y * params.cellSize + params.lineWidth / 2 + 1;
        ctx.fillStyle = color === -1 ? params.cellEmptyColor : 
                        color === -2 ? params.cellDeffColor : 
                        params.shapesColors[color] || params.cellEmptyColor;
        ctx.fillRect(coordX, coordY, params.cellSize - params.lineWidth - 1, params.cellSize - params.lineWidth - 1);
        ctx.strokeStyle = params.gridColor;
        ctx.lineWidth = params.lineWidth;
        ctx.strokeRect(coordX, coordY, params.cellSize - params.lineWidth - 1, params.cellSize - params.lineWidth - 1);
    };

    const showFigure = (figure, ctx) => {
        for (let rowIndex = 0; rowIndex < params.rowCount; rowIndex++) {
            for (let cellIndex = 0; cellIndex < params.colCount; cellIndex++) {
                showCell(cellIndex, rowIndex, figure[rowIndex][cellIndex], ctx);
            }
        }
    };

    const showGameField = () => {
        canvas.setAttribute("width", params.cellSize * params.colCount);
        canvas.setAttribute("height", params.cellSize * params.rowCount);
        wrapper.append(canvas);
        showGrid(context);
        showButtons();
        showFigure(figure, context);
    };

    const showGrid = (ctx) => {
        ctx.strokeStyle = params.gridColor;
        ctx.lineWidth = params.lineWidth;
        for (let colNum = 0; colNum <= params.colCount; colNum++) {
            const x = colNum * params.cellSize;
            ctx.beginPath();
            ctx.moveTo(x, 0);
            ctx.lineTo(x, params.rowCount * params.cellSize);
            ctx.stroke();
        }
        for (let rowNum = 0; rowNum <= params.rowCount; rowNum++) {
            const y = rowNum * params.cellSize;
            ctx.beginPath();
            ctx.moveTo(0, y);
            ctx.lineTo(params.colCount * params.cellSize, y);
            ctx.stroke();
        }
    };

    const generateRandomFigure = (k) => {
        const figure = prepareEmptyField();
        for (let i = 0; i < k; i++) {
            let x, y;
            do {
                x = Math.floor(Math.random() * params.colCount);
                y = Math.floor(Math.random() * params.rowCount);
            } while (figure[y][x] !== -1); 
            figure[y][x] = 0;
        }
        return figure;
    };

    const rotateFigure = (figure) => {
        if (figure === null) return null;
        const newFigure = prepareEmptyField();
        for (let y = 0; y < params.rowCount; y++) {
            for (let x = 0; x < params.colCount; x++) {
                newFigure[x][params.rowCount - y - 1] = figure[y][x];
            }
        }
        return newFigure;
    };

    const reflectFigure = (figure) => {
        if (figure === null) return null;
        const newFigure = prepareEmptyField();
        for (let y = 0; y < params.rowCount; y++) {
            for (let x = 0; x < params.colCount; x++) {
                newFigure[y][params.colCount - x - 1] = figure[y][x];
            }
        }
        return newFigure;
    };

    const randomShift = (figure) => {
        if (figure === null) return null;
        let sucess = false;
        let countTry = 0;
        while (!sucess) {
            sucess = true;
            let newFigure = prepareEmptyField();
            const dx = Math.floor(Math.random() * params.rowCount);
            const dy = Math.floor(Math.random() * params.colCount);
            for (let y = 0; y < params.rowCount; y++) {
                for (let x = 0; x < params.colCount; x++) {
                    let newX = Math.floor(x + dx) % params.rowCount;
                    let newY = Math.floor(y + dy) % params.colCount
                    newFigure[y][x] = figure[newY][newX];
                    if (newFigure[y][x] > -1 && (x + dx >= params.rowCount || y + dy >= params.colCount))
                    {
                        sucess = false;
                    }
                }
            }
            if (sucess) return newFigure;
            if (countTry > 50) return null;
            countTry++;
        }
    }

    const hasOverlap = (figure1, figure2) => {
        for (let y = 0; y < params.rowCount; y++) {
            for (let x = 0; x < params.colCount; x++) {
                if (figure1[y][x] !== -1 && figure2[y][x] !== -1) {
                    return true;
                }
            }
        }
        return false;
    };

    const updateCanvasSize = () => {
        canvas.setAttribute("width", params.cellSize * params.colCount);
        canvas.setAttribute("height", params.cellSize * params.rowCount);
        showGrid(context);
        showFigure(figure, context);
    };

    const generateFigures = () => {
        let figures = [];
        let attempts = 0;
        const maxAttempts = 5000;

        while (figures.length < params.countShapes && attempts < maxAttempts) {
            const baseFigure = generateRandomFigure(params.countCellsInShape);
            figures = [baseFigure];

            for (let i = 1; i < params.countShapes; i++) {
                let newFigure;
                if (Math.random() < 0.5) {
                    newFigure = reflectFigure(figures[i - 1]);
                } else {
                    newFigure = rotateFigure(figures[i - 1]); 
                }

                if (newFigure === null) {
                    break;
                }
                newFigure = randomShift(newFigure);
                if (newFigure === null) {
                    break;
                }
                let hasConflict = false;
                for (const fig of figures) {
                    if (hasOverlap(fig, newFigure)) {
                        hasConflict = true;
                        break;
                    }
                }

                if (!hasConflict) {
                    figures.push(newFigure);
                } else {
                    break;
                }
            }

            attempts++;
        }

        if (attempts >= maxAttempts) {
            return null;
        }

        return figures;
    };

    const showAnswer = () => {
        isViewingAnswer = true;
        figure = prepareEmptyField();
        answerFigures.forEach((fig, index) => {
            for (let y = 0; y < params.rowCount; y++) {
                for (let x = 0; x < params.colCount; x++) {
                    if (fig[y][x] !== -1) {
                        figure[y][x] = index % params.countShapes;
                    }
                }
            }
        });
        showFigure(figure, context);
        updateButtons();
        correctAnswersInRow = 0;
        showMessage("Вы сдались. Счетчик обнулен.");
        saveGameState();
    };

    const clearField = () => {
        figure = prepareEmptyField();
        answerFigures.forEach((fig) => {
            for (let y = 0; y < params.rowCount; y++) {
                for (let x = 0; x < params.colCount; x++) {
                    if (fig[y][x] !== -1) {
                        figure[y][x] = -2;
                    }
                }
            }
        });
        showFigure(figure, context);
        saveGameState();
    };

    const generateNewFigure = () => {
        isViewingAnswer = false;
        answerFigures = generateFigures();
        if (!answerFigures) {
            params.colCount++;
            params.rowCount++;

            figure = prepareEmptyField();
            if (answerFigures) {
                answerFigures = answerFigures.map(fig => {
                    const newFig = prepareEmptyField();
                    for (let y = 0; y < params.rowCount - 1; y++) {
                        for (let x = 0; x < params.colCount - 1; x++) {
                            if (fig[y] && fig[y][x] !== undefined) {
                                newFig[y][x] = fig[y][x];
                            }
                        }
                    }
                    return newFig;
                });
            }

            updateCanvasSize();
            answerFigures = generateFigures();
        }
        clearField();
        updateButtons();
        saveGameState();
    };

    const extractUserShapes = (userFigure) => {
        const shapes = {};
        for (let y = 0; y < params.rowCount; y++) {
            for (let x = 0; x < params.colCount; x++) {
                const color = userFigure[y][x];
                if (color !== -1 && color !== -2) {
                    if (!shapes[color]) {
                        shapes[color] = prepareEmptyField();
                    }
                    shapes[color][y][x] = 1;
                }
            }
        }
        return Object.values(shapes);
    };

    const normalizeShape = (shape) => {
        const newShape = prepareEmptyField();
        let minX = Infinity;
        let minY = Infinity;
        for (let y = 0; y < params.rowCount; y++) {
            for (let x = 0; x < params.colCount; x++) {
                if (shape[y][x] === 1) {
                    if (x < minX) minX = x;
                    if (y < minY) minY = y;
                }
            }
        }
        for (let y = 0; y < params.rowCount; y++) {
            for (let x = 0; x < params.colCount; x++) {
                if (shape[y][x] === 1) {
                    newShape[y - minY][x - minX] = 1;
                }
            }
        }

        return newShape;
    };

    const areShapesEqual = (shape1, shape2) => {
        const normalizedShape1 = normalizeShape(shape1);
        const isEqual = (s1, s2) => {
            for (let y = 0; y < params.rowCount; y++) {
                for (let x = 0; x < params.colCount; x++) {
                    if (s1[y][x] !== s2[y][x]) return false;
                }
            }
            return true;
        };

        let rotatedShape = shape2;
        for (let i = 0; i < 4; i++) {
            const normalizedRotatedShape = normalizeShape(rotatedShape);
            if (isEqual(normalizedShape1, normalizedRotatedShape)) return true;
            rotatedShape = rotateFigure(rotatedShape);
        }

        let reflectedShape = reflectFigure(shape2);
        for (let i = 0; i < 4; i++) {
            const normalizedReflectedShape = normalizeShape(reflectedShape);
            if (isEqual(normalizedShape1, normalizedReflectedShape)) return true;
            reflectedShape = rotateFigure(reflectedShape);
        }

        return false;
    };

    const checkAnswer = () => {
        const userFigure = JSON.parse(JSON.stringify(figure));
        for (let y = 0; y < params.rowCount; y++) {
            for (let x = 0; x < params.colCount; x++) {
                if (userFigure[y][x] === -2) {
                    showMessage("Не все клетки раскрашены.");
                    correctAnswersInRow = 0; 
                    return;
                }
            }
        }

        const userShapes = extractUserShapes(userFigure);
        const normalizedShapes = userShapes.map(normalizeShape);
        const firstShape = normalizedShapes[0];
        const allShapesEqual = normalizedShapes.every(shape => areShapesEqual(firstShape, shape));

        if (!allShapesEqual) {
            showMessage("Неправильно. Фигуры не равны.");
            correctAnswersInRow = 0;
            return;
        }
        showMessage("Правильно!");
        correctAnswersInRow++;
        if (correctAnswersInRow === 3) {
            correctAnswersInRow = 0;
            increaseDifficulty();
        }

        generateNewFigure();
        saveGameState();
    };

    const increaseDifficulty = () => {
        const totalCells = params.colCount * params.rowCount;
        const figureCells = (params.countShapes + 1) * (params.countCellsInShape + 1);

        if (figureCells > totalCells * 0.8) {
            params.colCount++;
            params.rowCount++;

            figure = prepareEmptyField();

            if (answerFigures) {
                answerFigures = answerFigures.map(fig => {
                    const newFig = prepareEmptyField();
                    for (let y = 0; y < params.rowCount - 1; y++) {
                        for (let x = 0; x < params.colCount - 1; x++) {
                            if (fig[y] && fig[y][x] !== undefined) {
                                newFig[y][x] = fig[y][x];
                            }
                        }
                    }
                    return newFig;
                });
            }

            showMessage("Уровень сложности повышен: увеличено поле.");
            updateCanvasSize();
        } else {
            const options = [
                () => {
                    params.countShapes++;
                    showMessage("Уровень сложности повышен: добавлена новая часть.");
                },
                () => {
                    params.countCellsInShape++;
                    showMessage("Уровень сложности повышен: увеличено количество клеток в частях.");
                },
                () => {
                    params.colCount++;
                    params.rowCount++;
                    figure = prepareEmptyField();
                    if (answerFigures) {
                        answerFigures = answerFigures.map(fig => {
                            const newFig = prepareEmptyField();
                            for (let y = 0; y < params.rowCount - 1; y++) {
                                for (let x = 0; x < params.colCount - 1; x++) {
                                    if (fig[y] && fig[y][x] !== undefined) {
                                        newFig[y][x] = fig[y][x];
                                    }
                                }
                            }
                            return newFig;
                        });
                    }

                    showMessage("Уровень сложности повышен: увеличено поле.");
                    updateCanvasSize();
                }
            ];
            const randomOption = options[Math.floor(Math.random() * options.length)];
            randomOption();
        }
        generateNewFigure();
        saveGameState();
    };

    const showMessage = (message) => {
        const messageElement = document.createElement("div");
        messageElement.classList.add("message");
        messageElement.innerHTML = message;
        wrapper.append(messageElement);
        setTimeout(() => messageElement.remove(), 3000);
    };

    const updateButtons = () => {
        const buttonList = wrapper.querySelector(".buttonList");
        if (!buttonList) return;
        buttonList.innerHTML = "";

        const taskText = document.createElement("div");
        taskText.innerHTML = `Разрежьте фигуру на ${params.countShapes} равных частей`;
        taskText.style.marginBottom = "10px";
        taskText.style.fontSize = "18px";
        taskText.style.fontWeight = "bold";
        buttonList.append(taskText);

        const buttonFill = document.createElement("button");
        buttonFill.classList.add("button-fill");
        buttonFill.innerHTML = "Change color";
        buttonFill.style.backgroundColor = params.shapesColors[shapeColor]; // Устанавливаем цвет кнопки
        buttonFill.addEventListener("click", () => {
            shapeColor = (shapeColor + 1) % params.countShapes;
            buttonFill.style.backgroundColor = params.shapesColors[shapeColor]; // Обновляем цвет кнопки
            saveGameState();
        });
        buttonList.append(buttonFill);

        if (isViewingAnswer) {
            const buttonGenerateNewFigure = document.createElement("button");
            buttonGenerateNewFigure.classList.add("button-generate");
            buttonGenerateNewFigure.innerHTML = "Generate New Figure";
            buttonGenerateNewFigure.addEventListener("click", generateNewFigure);
            buttonList.append(buttonGenerateNewFigure);
        } else {
            const buttonClear = document.createElement("button");
            buttonClear.classList.add("button-clear");
            buttonClear.innerHTML = "Clear";
            buttonClear.addEventListener("click", clearField);
            buttonList.append(buttonClear);
        }

        const buttonShowAnswer = document.createElement("button");
        buttonShowAnswer.classList.add("button-show-answer");
        buttonShowAnswer.innerHTML = "Show Answer";
        buttonShowAnswer.addEventListener("click", showAnswer);
        buttonList.append(buttonShowAnswer);

        const buttonCheckAnswer = document.createElement("button");
        buttonCheckAnswer.classList.add("button-check-answer");
        buttonCheckAnswer.innerHTML = "Check Answer";
        buttonCheckAnswer.addEventListener("click", checkAnswer);
        buttonList.append(buttonCheckAnswer);

        const stepsCounter = document.createElement("div");
        stepsCounter.innerHTML = `До следующего уровня: ${3 - correctAnswersInRow}`;
        stepsCounter.style.marginTop = "10px";
        stepsCounter.style.fontSize = "16px";
        buttonList.append(stepsCounter);
    };

    const showButtons = () => {
        if (!params.buttonList.length) return;

        const buttonList = document.createElement("div");
        buttonList.classList.add("buttonList");
        wrapper.prepend(buttonList); 
        updateButtons();
    };

    const initGame = () => {
        loadGameState();
        if (!answerFigures || answerFigures.length === 0) {
            answerFigures = generateFigures();
            figure = prepareEmptyField();
            answerFigures.forEach((fig) => {
                for (let y = 0; y < params.rowCount; y++) {
                    for (let x = 0; x < params.colCount; x++) {
                        if (fig[y][x] !== -1) {
                            figure[y][x] = -2;
                        }
                    }
                }
            });
        }
        showGameField();
    };

    initGame();
};