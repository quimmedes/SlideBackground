using System.Collections.Generic;
using UnityEngine;

public class InfiniteScroll : MonoBehaviour
{
    public float moveSpeed = 5f;            // Constant horizontal scrolling speed
    public float parallaxFactorX = 1f;      // Parallax factor for horizontal movement
    public float parallaxFactorY = 1f;      // Parallax factor for vertical movement
    private float imageWidth;               // Width of the image in world units
    private float imageHeight;              // Height of the image in world units

    private Camera mainCamera;
    private Vector3 lastCameraPosition;

    private static List<InfiniteScroll> scrollingImages = new List<InfiniteScroll>();

    void Start()
    {
        mainCamera = Camera.main;

        // Store the initial camera position
        lastCameraPosition = mainCamera.transform.position;

        // Calculate the width and height of the image in world units
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            imageWidth = spriteRenderer.bounds.size.x;
            imageHeight = spriteRenderer.bounds.size.y;
        }
        else
        {
            Debug.LogError("No SpriteRenderer found on the GameObject. Please attach a SpriteRenderer component.");
            return;
        }

        // Add this instance to the list of scrolling images
        scrollingImages.Add(this);
    }

    void OnDestroy()
    {
        // Remove this instance from the list when destroyed
        scrollingImages.Remove(this);
    }

    void Update()
    {
        // Calculate the camera's movement since the last frame
        Vector3 deltaCameraMovement = mainCamera.transform.position - lastCameraPosition;

        // Move the background to the left at constant speed
        transform.Translate(Vector3.left * moveSpeed * Time.deltaTime);

        // Adjust the background's position based on the camera's movement and parallax factors
        transform.position += new Vector3(
            deltaCameraMovement.x * parallaxFactorX,
            deltaCameraMovement.y * parallaxFactorY,
            0);

        // Update the last camera position for the next frame
        lastCameraPosition = mainCamera.transform.position;

        // Check if the background needs to be repositioned
        RepositionIfNeeded();
    }

    void RepositionIfNeeded()
    {
        // Calculate the horizontal and vertical extents of the camera's view in world units
        float cameraHorizontalExtent = mainCamera.orthographicSize * Screen.width / Screen.height;
        float cameraVerticalExtent = mainCamera.orthographicSize;

        // Calculate the left, right, top, and bottom edges of the camera's view
        float cameraLeftEdge = mainCamera.transform.position.x - cameraHorizontalExtent;
        float cameraRightEdge = mainCamera.transform.position.x + cameraHorizontalExtent;
        float cameraBottomEdge = mainCamera.transform.position.y - cameraVerticalExtent;
        float cameraTopEdge = mainCamera.transform.position.y + cameraVerticalExtent;

        bool repositioned = false;

        // Check horizontal repositioning
        if (transform.position.x + imageWidth / 2 < cameraLeftEdge)
        {
            RepositionToRight();
            repositioned = true;
        }
        else if (transform.position.x - imageWidth / 2 > cameraRightEdge)
        {
            RepositionToLeft();
            repositioned = true;
        }

        // Check vertical repositioning
        if (transform.position.y + imageHeight / 2 < cameraBottomEdge)
        {
            RepositionToTop();
            repositioned = true;
        }
        else if (transform.position.y - imageHeight / 2 > cameraTopEdge)
        {
            RepositionToBottom();
            repositioned = true;
        }

        // If repositioned, update the list to ensure correct ordering
        if (repositioned)
        {
            // Sort the list if necessary (optional, depending on your needs)
        }
    }

    void RepositionToRight()
    {
        // Find the rightmost image
        float rightmostX = GetRightmostXPosition();

        // Position this image to the right of the rightmost image
        Vector3 newPosition = transform.position;
        newPosition.x = rightmostX + imageWidth;
        transform.position = newPosition;
    }

    void RepositionToLeft()
    {
        // Find the leftmost image
        float leftmostX = GetLeftmostXPosition();

        // Position this image to the left of the leftmost image
        Vector3 newPosition = transform.position;
        newPosition.x = leftmostX - imageWidth;
        transform.position = newPosition;
    }

    void RepositionToTop()
    {
        // Find the topmost image
        float topmostY = GetTopmostYPosition();

        // Position this image above the topmost image
        Vector3 newPosition = transform.position;
        newPosition.y = topmostY + imageHeight;
        transform.position = newPosition;
    }

    void RepositionToBottom()
    {
        // Find the bottommost image
        float bottommostY = GetBottommostYPosition();

        // Position this image below the bottommost image
        Vector3 newPosition = transform.position;
        newPosition.y = bottommostY - imageHeight;
        transform.position = newPosition;
    }

    float GetRightmostXPosition()
    {
        float maxX = float.MinValue;

        // Use the cached list of scrolling images
        foreach (InfiniteScroll image in scrollingImages)
        {
            float imageX = image.transform.position.x;
            if (imageX > maxX)
            {
                maxX = imageX;
            }
        }
        return maxX;
    }

    float GetLeftmostXPosition()
    {
        float minX = float.MaxValue;

        // Use the cached list of scrolling images
        foreach (InfiniteScroll image in scrollingImages)
        {
            float imageX = image.transform.position.x;
            if (imageX < minX)
            {
                minX = imageX;
            }
        }
        return minX;
    }

    float GetTopmostYPosition()
    {
        float maxY = float.MinValue;

        // Use the cached list of scrolling images
        foreach (InfiniteScroll image in scrollingImages)
        {
            float imageY = image.transform.position.y;
            if (imageY > maxY)
            {
                maxY = imageY;
            }
        }
        return maxY;
    }

    float GetBottommostYPosition()
    {
        float minY = float.MaxValue;

        // Use the cached list of scrolling images
        foreach (InfiniteScroll image in scrollingImages)
        {
            float imageY = image.transform.position.y;
            if (imageY < minY)
            {
                minY = imageY;
            }
        }
        return minY;
    }
}
