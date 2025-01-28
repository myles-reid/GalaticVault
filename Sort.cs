namespace GalaticVault {

	/*
	 * I decided to put the sorting method into its own class due to the fact that I went with a merge sort algorithm.
	 * Because it is such a large method with a lot of weird parts, I figured keeping it seperate would allow for a cleaner
	 * program.cs file, and would allow for easier debugging if something went wrong.
	 * I commented inside to explain what was going on, more for my own use at a later date.
	 */
	public abstract class Sort {
		private static void merge(Artifact[] arr, int low, int mid, int hi) {
			// Find sizes of two subarrays to be merged
			int n1 = mid - low + 1;
			int n2 = hi - mid;

			// Create temp arrays
			Artifact[] left = new Artifact[n1];
			Artifact[] right = new Artifact[n2];
			int i, j;

			// Copy data to temp arrays
			for (i = 0; i < n1; ++i) { left[i] = arr[low + i]; }
			for (j = 0; j < n2; ++j) { right[j] = arr[mid + 1 + j]; }

			// Merge the temp array
			// Initial indexes of first and second subarrays
			i = j = 0;

			// Initial index of merged subarray array
			int k = low;
			while (i < n1 && j < n2) {
				string leftName = left[i].GetName().ToLower();
				string rightName = right[j].GetName().ToLower();
				if (leftName.CompareTo(rightName) <= 0) {
					arr[k] = left[i];
					i++;
				} else {
					arr[k] = right[j];
					j++;
				}
				k++;
			}

			// Copy remaining elements of left[] if any
			while (i < n1) {
				arr[k] = left[i];
				i++;
				k++;
			}

			// Copy remaining elements of right[] if any
			while (j < n2) {
				arr[k] = right[j];
				j++;
				k++;
			}
		}

		public static void MergeSort(Artifact[] arr, int low, int hi) {
			if (low < hi) {

				// Find the middle point
				int mid = low + (hi - low) / 2;

				// Sort first and second halves
				MergeSort(arr, low, mid);
				MergeSort(arr, mid + 1, hi);

				// Merge the sorted halves
				merge(arr, low, mid, hi);
			}
		}
	}
}
