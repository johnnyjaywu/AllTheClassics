using ContentContent.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace AllTheClassics.Concentration
{
	[RequireComponent(typeof(GridLayoutGroup))]
	public class ConcentrationBoard : MonoBehaviour
	{
		private RectTransform rectTransform;
		private GridLayoutGroup gridLayoutGroup;

		public void Clean()
		{
			gameObject.DestroyChildrenImmediate();
			gridLayoutGroup = GetComponent<GridLayoutGroup>();
			gridLayoutGroup.constraint = GridLayoutGroup.Constraint.Flexible;
		}

		public void InitializeBoard(int size)
		{
			if (size < 2) return;

			var dimension = FindMostSquareDimensions(size);
			SetCellSize(dimension.Item1, dimension.Item2);
		}

		private (int, int) FindMostSquareDimensions(int area)
		{
			if (area <= 0)
			{
				throw new ArgumentException("Area must be a positive integer.");
			}

			// Start with a potential width that is the ceiling of the square root
			int width = (int)Math.Ceiling(Math.Sqrt(area));

			// Iterate downwards until a divisor is found
			while (width > 0)
			{
				if (area % width == 0)
				{
					int height = area / width;
					return (width, height);
				}
				width--;
			}

			// This should not be reached for positive 'area' as 1 is always a divisor
			return default;
		}

		private void SetCellSize(int columns, int rows)
		{
			rectTransform = GetComponent<RectTransform>();
			gridLayoutGroup = GetComponent<GridLayoutGroup>();
			LayoutRebuilder.ForceRebuildLayoutImmediate(rectTransform);

			// Determine the working board size with the padding
			float width = rectTransform.rect.width - gridLayoutGroup.padding.horizontal;
			float height = rectTransform.rect.height - gridLayoutGroup.padding.vertical;
			float boardAspect = width / height;

			// Landscape
			if (boardAspect > 1)
			{	
				gridLayoutGroup.constraint = GridLayoutGroup.Constraint.FixedColumnCount; // Use fixed columns
				(columns, rows) = rows > columns ? (rows, columns) : (columns, rows); // Flip the columns and rows so they are landscape
				gridLayoutGroup.constraintCount = columns; // Set constraintCount to columns (the larger value)
			}
			else // Portrait
			{
				gridLayoutGroup.constraint = GridLayoutGroup.Constraint.FixedRowCount; // Use fixed rows
				(columns, rows) = columns > rows ? (rows, columns) : (columns, rows); // Flip the columns and rows so they are portrait
				gridLayoutGroup.constraintCount = rows; // Set constraintCount to rows (the larger value)
			}

			// Compare the aspect ratio to determine the cell size
			float gridAspect = (float)columns / rows;
			float cellSize = boardAspect > gridAspect ? height / rows : width / columns;

			// Consider the spacing
			gridLayoutGroup.cellSize = new Vector2(cellSize - gridLayoutGroup.spacing.x, cellSize - gridLayoutGroup.spacing.y);
		}
	}
}